using Microsoft.AspNetCore.SignalR;

namespace PositionsService.Hubs
{
    public class PositionHub : Hub
    {
        // Method used for sending the position number of the position inserted to the clients connected
        public async Task NotifyPositionUpdate(string position)
        {
            try
            {
                await Clients.All.SendAsync("PositionUpdated", position);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in NotifyPositionUpdate: {ex.Message}");
            }
        }
    }
}