using TooManyCooks.Common;

IReadOnlyList<Food> dishes =
[
	new Turkey(),
	new MashedPotatoes(),
	new Stuffing(),
	new Gravy()
];

Console.WriteLine("Cooking Started");

foreach (var food in dishes)
{
	await food.Cook();
	Console.WriteLine($"Finished Cooking {food.Name}");
}

Console.WriteLine("Ready to eat");