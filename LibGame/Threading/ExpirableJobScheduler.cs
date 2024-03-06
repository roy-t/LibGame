namespace LibGame.Threading;

/// <summary>
/// Runs a job on the thread pool whenever a caller indicates the data is out of date. If a caller
/// requests a newer version while the job is running, the job will be cancelled.
///
/// Allows callers to access data when it is up to date, without having to worry it will be modified
/// while doing so.
///
///
/// Example: use this class to guard accessing the render data for a deformable terrain. Call RunIfOutOfDate
/// every frame and the ExpirableJobScheduler will make sure that a job starts whenever the data is out of date.
/// If the deformable terrain changes again while the job is running, the ExpirableJobScheduler and will cancel
/// the current job and schedule  anew run next frame.
///
/// Whenever a new version of the render data for the deformable terrain needs to uploaded to the GPU call DoIfUpToDate
/// until it calls the action parameter. This way you are sure you access the render data while no job is updating it.
///
/// This class is thread safe.
/// </summary>
public sealed class ExpirableJobScheduler
{
    private readonly Action<CancellationToken> Job;

    private CancellationTokenSource? CancellationTokenSource;
    private int latestCompletedVersion;
    private int currentRunningVersion;

    private readonly SemaphoreSlim QueueSemaphore;
    private readonly SemaphoreSlim RunningSemaphore;

    public ExpirableJobScheduler(Action<CancellationToken> job)
    {
        this.Job = job;

        this.QueueSemaphore = new SemaphoreSlim(1, 1);
        this.RunningSemaphore = new SemaphoreSlim(1, 1);
    }

    /// <summary>
    /// Calls the action delegate if the current version is up to date and no jobs are currently running.
    /// This method is thread safe.
    /// </summary>
    public bool DoIfUpToDate(int minVersion, Action action)
    {
        try
        {
            this.QueueSemaphore.Wait();
            if (this.IsIdleAndFreshThreadUnsafe(minVersion))
            {
                action();
                return true;
            }
            return false;
        }
        finally
        {
            this.QueueSemaphore.Release();
        }
    }

    /// <summary>
    /// Makes sure current version is up-to-date, or that a job is running to become up-to-date.
    /// This method is thread safe and should be called every frame until your call to DoIfUpdate succeeds.
    /// </summary>
    public void RunIfOutOfDate(int minVersion)
    {
        try
        {
            this.QueueSemaphore.Wait();

            if (this.IsIdleAndFreshThreadUnsafe(minVersion))
            {
                return;
            }
            else if (this.IsRunningAndFreshThreadUnsafe(minVersion))
            {
                return;
            }
            else if (this.IsRunningAndStaleThreadUnsafe(minVersion))
            {
                this.CancellationTokenSource!.Cancel();
            }
            else if (this.IsIdleAndStaleThreadUnsafe(minVersion))
            {
                this.ScheduleJob(minVersion);
            }
        }
        finally
        {
            this.QueueSemaphore.Release();
        }
    }


    private void ScheduleJob(int version)
    {
        try
        {
            this.RunningSemaphore.Wait();
            this.currentRunningVersion = version;

            this.CancellationTokenSource = new CancellationTokenSource();

            var token = this.CancellationTokenSource.Token;

            this.Job(token);

            if (!token.IsCancellationRequested)
            {
                this.latestCompletedVersion = version;
            }

        }
        finally
        {
            this.CancellationTokenSource!.Dispose();
            this.CancellationTokenSource = null;

            this.RunningSemaphore.Release();
        }
    }

    private bool IsIdleAndFreshThreadUnsafe(int minVersion)
    {
        return this.latestCompletedVersion >= minVersion && this.RunningSemaphore.CurrentCount > 0;
    }

    private bool IsIdleAndStaleThreadUnsafe(int minVersion)
    {
        return this.latestCompletedVersion < minVersion && this.RunningSemaphore.CurrentCount > 0;
    }

    private bool IsRunningAndFreshThreadUnsafe(int minVersion)
    {
        return this.currentRunningVersion >= minVersion && this.RunningSemaphore.CurrentCount == 0;
    }

    private bool IsRunningAndStaleThreadUnsafe(int minVersion)
    {
        return this.currentRunningVersion < minVersion && this.RunningSemaphore.CurrentCount == 0;
    }

}
