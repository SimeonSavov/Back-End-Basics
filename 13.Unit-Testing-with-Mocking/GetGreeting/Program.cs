namespace GetGreeting
{
    class Program
    {
        static void Main(string[] args)
        {
            GreetingProvider greetingProvider = new GreetingProvider(new FakeTimeProvider(new DateTime(2023, 4, 5)));
            string greeting = greetingProvider.GetGreeting();
            Console.WriteLine(greeting);
        }
    }

}