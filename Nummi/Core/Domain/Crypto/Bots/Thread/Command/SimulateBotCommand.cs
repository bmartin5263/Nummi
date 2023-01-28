namespace Nummi.Core.Domain.Crypto.Bots.Thread.Command; 

public class SimulateBotCommand : ICommand {
    private SimulationParameters Parameters { get; }
    private string ResultId { get; }

    public SimulateBotCommand(SimulationParameters parameters, string resultId) {
        Parameters = parameters;
        ResultId = resultId;
    }

    public void Execute(BotThread.BotThreadController controller) {
        controller.SimulateBot(Parameters, ResultId);
    }
}