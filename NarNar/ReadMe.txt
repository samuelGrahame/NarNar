// 

system class Customer
{
	public int Id;
	public string LastName;
	public string FirstName;		
}

server Customer Get()
{
	return new Customer()
	{
		Id = 1,
		LastName = "LastName",
		FirstName = "FirstName"	
	};
}

client static void Main()
{
	var cust = Get();

	Console.WriteLine($"{cust.LastName} {cust.FirstName}");
}