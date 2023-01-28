namespace Nummi.Core.Domain.Crypto.Bots; 

public class SimulationParameters {
    public DateTime StartTime { get; private init; }
    public DateTime EndTime { get; private init; }

    private SimulationParameters() {
    }

    public SimulationParameters(DateTime startTime, DateTime endTime) {
        StartTime = startTime.Kind != DateTimeKind.Local ? DateTime.SpecifyKind(startTime, DateTimeKind.Local) : startTime;
        EndTime = endTime.Kind != DateTimeKind.Local ? DateTime.SpecifyKind(endTime, DateTimeKind.Local) : endTime;
    }
}