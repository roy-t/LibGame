using LibGame.Miscellaneous;

namespace LibGame.Tests.Miscellaneous;

public static class CoffmanGrahamTests
{
    private record class ProducerConsumer(int Produces, int Consumes);
    private record class Product(int Id);

    private sealed class RelationDescriber : IRelationDescriber<ProducerConsumer, Product>
    {
        public IReadOnlyList<Product> GetConsumption(ProducerConsumer item)
        {
            if (item.Consumes == 0)
            {
                return Array.Empty<Product>();
            }

            return new[] { new Product(item.Consumes) };
        }

        public IReadOnlyList<Product> GetProduction(ProducerConsumer item)
        {
            return new[] { new Product(item.Produces) };
        }
    }

    [Fact(DisplayName = "Ordering a list of items should put them in an order where their dependencies are honored")]
    public static void Order()
    {
        var describer = new RelationDescriber();
        var algorithm = new CoffmanGraham<ProducerConsumer, Product>(describer);


        var items = new[]
        {
            new ProducerConsumer(3, 2),
            new ProducerConsumer(1, 0),
            new ProducerConsumer(0, 1),
            new ProducerConsumer(2, 1),
            new ProducerConsumer(3, 1),
        };

        var ordered = algorithm.Order(items);

        var produced = new HashSet<int>();

        foreach (var item in ordered)
        {
            if (item.Consumes != 0)
            {
                Contains(item.Consumes, produced);
            }

            produced.Add(item.Produces);
        }
    }
}
