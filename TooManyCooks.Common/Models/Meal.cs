namespace TooManyCooks.Common;

public abstract class Meal(params ReadOnlySpan<Food> ingredients)
{
	public virtual Task Cook() => Task.WhenAll(Ingredients.Select(static ingredient => ingredient.Cook()));

	public IReadOnlyList<Food> Ingredients { get; } = [.. ingredients];
}

public class ThanksgivingDinner() : Meal(new Gravy(), new Stuffing(), new Turkey(), new MashedPotatoes());