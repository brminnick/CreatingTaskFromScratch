using TooManyCooks.Common;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

	var turkey = new Turkey();
	var gravy = new Gravy();
	
	await Task.WhenAll(turkey.Cook(), gravy.Cook());
	
	Console.WriteLine("Ready to eat");

	Console.ReadLine();

#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed