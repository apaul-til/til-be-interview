class Program
{
  static async Task Main(string[] args)
  {
    Connection connection = new Connection();
    await connection.run();
  }

}
