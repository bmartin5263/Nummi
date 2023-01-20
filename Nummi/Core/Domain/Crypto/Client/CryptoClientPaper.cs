using Nummi.Core.Database;
using Nummi.Core.External.Binance;

namespace Nummi.Core.Domain.Crypto.Client; 

public class CryptoClientPaper : CryptoClientCommon {
    public CryptoClientPaper(BinanceClient binanceClient, AppDb appDb) : base(binanceClient, appDb) { }
}