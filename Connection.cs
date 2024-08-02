using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

class Connection
{
  Uri Uri = new Uri("wss://api-sandbox.theindoorlab.com/v4/trackframes");
  Region SquareRegion;

  JsonSerializerOptions SerializerOptions;

  public Connection()
  {
    Coordinate[] coords = {
      new Coordinate(15, 15),
      new Coordinate(15, -15),
      new Coordinate(-15, -15),
      new Coordinate(-15, 15)
    };
    this.SquareRegion = new Region(coords);
    this.SerializerOptions = new JsonSerializerOptions
    {
      Converters = { new IsoDateTimeConverter() },
      PropertyNameCaseInsensitive = true 
    };
  }

  public async Task run()
  {
    using (ClientWebSocket client = new ClientWebSocket())
    {
      await client.ConnectAsync(this.Uri, CancellationToken.None);

      /*
        Send a message to the WebSocket server, you can specify which trackframe you desire.
        i.e. index value of 10 will give you the 10th trackframe.
      */
      int number = 0;
      await SendMessage(client, number);

      // Start a background task to receive messages
      Task receiveTask = RecieveMessage(client);

      await receiveTask;
    }
  }

  public async Task SendMessage(ClientWebSocket client, int index)
  {
    byte[] bytes = Encoding.UTF8.GetBytes(index.ToString());
    await client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
  }

  public async Task RecieveMessage(ClientWebSocket client)
  {
    byte[] buffer = new byte[4096];
    var messageBuffer = new StringBuilder();

    while (client.State == WebSocketState.Open)
    {
      WebSocketReceiveResult result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

      if (result.MessageType == WebSocketMessageType.Text)
      {
        string receivedMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
        messageBuffer.Append(receivedMessage);

        if (result.EndOfMessage)
        {
          string json = messageBuffer.ToString();
          messageBuffer.Clear();

          TrackFrame trackFrame = JsonSerializer.Deserialize<TrackFrame>(json, this.SerializerOptions);
          this.PrintTrackFrame(trackFrame);

          /*
            Handle Trackfames here
          */


        }
      }
    }
  }

  // Create any data structures you need 
  
  //Print trackframes
  public void PrintTrackFrame(TrackFrame trackFrame)
  {
    // Write your code here 
    Console.WriteLine($"Timestamp: {trackFrame.Timestamp}");
    Console.WriteLine("Tracks:");

    foreach (Track trackedObject in trackFrame.Tracks)
    {
      Console.WriteLine($"  TrackedObjectId: {trackedObject.TrackedObjectId}");
      Console.WriteLine($"  X: {trackedObject.X}");
      Console.WriteLine($"  Y: {trackedObject.Y}");
    }
  }
}