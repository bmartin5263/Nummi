namespace NummiTests.Utils; 

public static class UnitTestUtils {

    public static TestTransaction CreateTestTransaction() {
        return new TestTransaction {
            BarRepository = new BarTestRepository(),
            BotRepository = new BotTestRepository(),
            SimulationRepository = new SimulationTestRepository(),
            StrategyRepository = new StrategyTestRepository(),
            StrategyTemplateRepository = new StrategyTemplateTestRepository(),
            UserRepository = new UserTestRepository()
        };
    }
    
}