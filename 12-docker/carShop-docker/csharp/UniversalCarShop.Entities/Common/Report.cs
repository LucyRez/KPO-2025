namespace UniversalCarShop.Entities.Common;

public record Report(string Title, string Content)
{
    public override string ToString()
    {
        return $"{Title}\n\n{Content}";
    }
}


