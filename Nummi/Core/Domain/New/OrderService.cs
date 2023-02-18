// using Nummi.Core.Domain.Crypto.Bots;
// using Nummi.Core.Domain.Crypto.Strategies;
// using Nummi.Core.Exceptions;
// using Nummi.Core.Util;
//
// namespace Nummi.Core.Domain.Crypto.Ordering;
//
// public class OrderService {
//
//     private TradingContextFactory TradingContextFactory { get; }
//     private BotService BotService { get; }
//
//     public OrderService(TradingContextFactory tradingContextFactory, BotService botService) {
//         TradingContextFactory = tradingContextFactory;
//         BotService = botService;
//     }
//
//     public Order PlaceOrder(string botId, OrderRequest request) {
//         var bot = BotService.GetBotById(botId).OrElseThrow(() => EntityNotFoundException<Bot>.IdNotFound(botId));
//         switch (bot.Mode) {
//             // case TradingMode.Paper:
//             //     var context = TradingContextFactory.CreateRealtime(bot);
//             //     return context.PlaceOrder(request);
//             default:
//                 throw new ArgumentOutOfRangeException(nameof(bot.Mode), bot.Mode, null);
//         }
//     }
//
// }