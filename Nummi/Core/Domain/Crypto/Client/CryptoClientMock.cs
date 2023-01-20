using Nummi.Core.Database;
using Nummi.Core.External.Binance;

namespace Nummi.Core.Domain.Crypto.Client; 

public class CryptoClientMock : CryptoClientCommon {
    public CryptoClientMock(BinanceClient binanceClient, AppDb appDb) : base(binanceClient, appDb) { }
}