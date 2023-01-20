using Nummi.Core.Database;
using Nummi.Core.External.Binance;

namespace Nummi.Core.Domain.Crypto.Client; 

public class CryptoClientLive : CryptoClientCommon {
    public CryptoClientLive(BinanceClient binanceClient, AppDb appDb) : base(binanceClient, appDb) { }
}