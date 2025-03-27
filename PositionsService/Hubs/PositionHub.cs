using Microsoft.AspNetCore.SignalR;

namespace PositionsService.Hubs
{
    public class PositionHub : Hub
    {
        public async Task NotifyPositionInsert(string position)
        {
            try
            {
                await Clients.All.SendAsync("PositionInserted", position);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in NotifyPositionInsert: {ex.Message}");
            }
        }

        // Method used for sending the position number of the position updated
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

        public async Task NotifyPositionDelete(string position)
        {
            try
            {
                await Clients.All.SendAsync("PositionDeleted", position);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in NotifyPositionDelete: {ex.Message}");
            }
        }
    }
}