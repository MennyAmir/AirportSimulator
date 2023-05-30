namespace AirportSimulator.Simulator.Models
{
    public class Simulator
    {
        public int id = 1;
        public string url = "https://localhost:7235/api/Airport/";
        HttpClient httpClient = new HttpClient(); 
        Random rnd = new Random();

        public async Task SandAirplans() {
            while (true)
            {
                int num = rnd.Next(0,101);

                if (num < 65) { 
                
                    var sandAirplan = await httpClient.GetAsync(url + "Incoming/" + id);
                    while (!sandAirplan.IsSuccessStatusCode)
                    {
                        await Task.Delay(2000);
                        sandAirplan = await httpClient.GetAsync(url + "Incoming/" + id);

                    }

                }
                else
                {
                    var sandAirplan = await httpClient.GetAsync(url + "Departing/" + id);
                    while (!sandAirplan.IsSuccessStatusCode)
                    {
                        await Task.Delay(2000);
                        sandAirplan = await httpClient.GetAsync(url + "Departing/" + id);

                    }
                }
                id++;
                await Task.Delay(4000);
            }
        }
    }
}
