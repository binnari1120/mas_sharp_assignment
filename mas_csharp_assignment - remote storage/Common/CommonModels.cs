using System.Diagnostics;
using System.Drawing;

namespace CommonModels
{
    public class Converter
    {
        public static Exchange get_exchange(string exchange)
        {
            switch (exchange.ToUpper())
            {
                case "미래에셋":
                    return Exchange.MIRAE_ASSET;
            }

            return default(Exchange);
        }

        public static Side get_side(string side)
        {
            switch (side.ToUpper())
            {
                case "1" or "매수":
                    return Side.BUY;
                case "2" or "매도":
                    return Side.SELL;
            }

            return default(Side);
        }

        public static OrderExecutionResult get_order_execution_result(string order_execution_result)
        {
            switch (order_execution_result.ToUpper())
            {
                case "1":
                    return OrderExecutionResult.CREATED;
                case "2":
                    return OrderExecutionResult.FILLED;
                case "3":
                    return OrderExecutionResult.CANCELED;
            }

            return default(OrderExecutionResult);
        }
    }

    public enum Exchange
    {
        NONE,
        MIRAE_ASSET
    }

    public enum Side
    {
        NONE,
        BUY,
        SELL
    }

    public enum OrderExecutionResult
    {
        NONE,
        CREATED,
        FILLED,
        //PARTIALLY_FILLED,
        CANCELED,
        FAILED
    }

    public struct SymbolConstraint
    {
        public string code;
        public string symbol;
        public decimal price_step;
        public int quantity_step;
        public decimal initial_price;

        public override string ToString()
        {
            string message = $"  ◻️  종목명: {symbol} (코드: {code})";
            message = message + $"\n   - 호가단위(원): {price_step}";
            message = message + $"\n   - 수량단위(주): {quantity_step}";
            message = message + $"\n   - 시초가(원): {initial_price}";

            return message;
        }
    }

    public struct Depth
    {
        public decimal price;
        public decimal quantity;

        public override string ToString()
        {
            string message = $"\n가격: {price} |  수량: {quantity}";

            return message;
        }
    }

    public struct Orderbook
    {
        public string code;
        public string symbol;
        public List<Depth> asks;
        public List<Depth> bids;
        public decimal last_price;

        public override string ToString()
        {
            string message = $"종목명: {symbol} (코드: {code})";

            int i = 0;

            message = message + $"\n매도호가/수량";
            foreach (Depth depth in asks)
            {
                message = message + $"\n#{i}: {depth}";
            }

            i = 0;

            message = message + $"\n매수호가/수량";
            foreach (Depth depth in bids)
            {
                message = message + $"\n#{i}: {depth}";
            }

            message = message + $"\n체결가: {last_price}";

            return message;
        }
    }

    public struct Asset
    {
        public Exchange exchange;
        public decimal initial_balance;
        public decimal total_position_size;
        public decimal total_order_size;
        public decimal available_balance;
        public decimal pnl;
        public decimal equity;

        public override string ToString()
        {
            string message = $"  ◻️  거래소: {exchange}";
            message = message + $"\n   - 원금:(원) {initial_balance}";
            message = message + $"\n   - 총 매입금액(원): {total_position_size}";
            message = message + $"\n   - 총 주문금액(원): {total_order_size}";
            message = message + $"\n   - 주문가능잔고(원): {available_balance}";
            message = message + $"\n   - 수익(원): {pnl}";
            message = message + $"\n   - 평가잔고(원): {equity}";

            return message;
        }
    };

    public struct Position
    {
        public Exchange exchange;
        public string symbol;
        public string code;
        public decimal average_price;
        public int quantity;
        public decimal size;

        public override string ToString()
        {
            string message = $"  ◻️  거래소: {exchange}";
            message = message + $"\n   - 종목명: {symbol}";
            message = message + $"\n   - 코드: {code}";
            message = message + $"\n   - 평균가격(원): {average_price}";
            message = message + $"\n   - 보유수량(주): {quantity}";
            message = message + $"\n   - 매입금액(원): {size}";

            return message;
        }
    }

    public struct Order
    {
        public Exchange exchange;
        public string id;
        public string symbol;
        public string code;
        public Side side;
        public decimal price;
        public int quantity;
        public decimal size;

        public override string ToString()
        {
            string message = $"  ◻️  거래소: {exchange}";
            message = message + $"\n   - 종목명: {symbol}";
            message = message + $"\n   - 코드: {code}";
            message = message + $"\n   - 구분: {side}";
            message = message + $"\n   - 주문가격(원): {price}";
            message = message + $"\n   - 주문수량(주): {quantity}";
            message = message + $"\n   - 주문금액(원): {size}";

            return message;
        }
    }

    public struct TransactionRecord
    {
        public Exchange exchange;
        public DateTime time;
        public string symbol;
        public string code;
        public decimal executed_price;
        public decimal executed_quantity;
        public decimal executed_size;
        public decimal realized_profit;

        public override string ToString()
        {
            string message = $"  ◻️  거래소: {exchange}";
            message = message + $"\n   - 시간: {time}";
            message = message + $"\n   - 종목명: {symbol}";
            message = message + $"\n   - 코드: {code}";
            message = message + $"\n   - 체결가격(원): {executed_price}";
            message = message + $"\n   - 체결수량(주):  {executed_quantity}";
            message = message + $"\n   - 매입금액(원):  {executed_size}";
            message = message + $"\n   - 실현손익(원):  {realized_profit}";

            return message;
        }
    }

    public struct OrderRecord
    {
        public Exchange exchange;
        public DateTime time;
        public string symbol;
        public string code;
        public Side side;
        public decimal price;
        public int quantity;
        public decimal size;
        public OrderExecutionResult result;
        public string error_message;

        public override string ToString()
        {
            string message = $"  ◻️  거래소: {exchange}";
            message = message + $"\n   - 시간: {time}";
            message = message + $"\n   - 종목명: {symbol}";
            message = message + $"\n   - 코드: {code}";
            message = message + $"\n   - 구분: {side}";
            message = message + $"\n   - 주문가격(원): {price}";
            message = message + $"\n   - 주문수량(주): {quantity}";
            message = message + $"\n   - 주문금액(원):  {size}";

            if (!result.Equals(default(OrderExecutionResult))) message = message + $"\n   - 결과: {result}";
            if (!string.IsNullOrEmpty(error_message)) message = message + $"\n   - 사유: {error_message}";

            return message;
        }
    }

    public struct OrderApplication
    {
        public Exchange exchange;
        public string id;
        public string symbol;
        public string code;
        public Side side;
        public decimal price;
        public int quantity;
        public OrderExecutionResult result;
        public string error_message;

        public override string ToString()
        {
            string message = $"  ◻️  거래소: {exchange}";
            if (!string.IsNullOrEmpty(id)) message = message + $"\n   - 주문번호: {id}";
            message = message + $"\n   - 종목명: {symbol}";
            message = message + $"\n   - 코드: {code}";
            message = message + $"\n   - 구분: {side}";
            message = message + $"\n   - 주문가격(원): {price}";
            message = message + $"\n   - 주문수량(주): {quantity}";
            if (!result.Equals(default(OrderExecutionResult))) message = message + $"\n   - 결과: {result}";
            if (!string.IsNullOrEmpty(error_message)) message = message + $"\n   - 사유: {error_message}";

            return message;
        }
    }
}
