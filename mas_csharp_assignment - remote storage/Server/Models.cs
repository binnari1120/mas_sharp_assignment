using CommonModels;

namespace Server
{
    public class Models
    {
        public static List<SymbolConstraint> all_symbol_constraints = new List<SymbolConstraint>
        {
            // Reference: Naver Financial (Top 10 market capitalization of KOSPI, opening price is the closing price on April 23, 2024)
            new SymbolConstraint() { symbol = "삼성전자", code = "005930", price_step = 100, quantity_step = 1, initial_price = 75500 },
            new SymbolConstraint() { symbol = "하이닉스", code = "000660", price_step = 100, quantity_step = 1, initial_price = 171000 },
            new SymbolConstraint() { symbol = "LG에너지솔루션", code = "373220", price_step = 500, quantity_step = 1, initial_price = 370000 },
            new SymbolConstraint() { symbol = "삼성바이오로직스", code = "207940", price_step = 100, quantity_step = 1, initial_price = 791000 },
            new SymbolConstraint() { symbol = "삼성전자우", code = "005935", price_step = 100, quantity_step = 1, initial_price = 63500 },
            new SymbolConstraint() { symbol = "현대차", code = "005380", price_step = 500, quantity_step = 1, initial_price = 249500 },
            new SymbolConstraint() { symbol = "기아", code = "000270", price_step = 100, quantity_step = 1, initial_price = 115900 },
            new SymbolConstraint() { symbol = "셀트리온", code = "068270", price_step = 100, quantity_step = 1, initial_price = 179700 },
            new SymbolConstraint() { symbol = "POSCO홀딩스", code = "005490", price_step = 500, quantity_step = 1, initial_price = 391500 },
            new SymbolConstraint() { symbol = "NAVER", code = "035420", price_step = 100, quantity_step = 1, initial_price = 180100 },
        };

        public static List<Orderbook> get_all_symbol_orderbooks()
        {
            List<Orderbook> orderbooks = new List<Orderbook>();

            foreach (SymbolConstraint symbol_constraint in all_symbol_constraints)
            {
                orderbooks.Add(_create_dynamic_orderbook(symbol_constraint));
            }

            return orderbooks;
        }

        private static Orderbook _create_dynamic_orderbook(SymbolConstraint symbol_constraint)
        {
            Random random = new Random();

            List<Depth> asks = new List<Depth>();
            List<Depth> bids = new List<Depth>();

            int deviation_factor = random.Next(0, 10);
            int precision = Convert.ToInt32(Math.Log10(Convert.ToDouble(symbol_constraint.price_step)));

            // asks
            for (int i = 0; i < 10; i++)
            {
                Depth ask = new Depth
                {
                    price = symbol_constraint.initial_price + symbol_constraint.price_step * (10 + i + deviation_factor),
                    quantity = random.Next(1000, 10000)
                };

                asks.Add(ask);
            }

            // bids
            for (int i = 0; i < 10; i++)
            {
                Depth bid = new Depth
                {
                    price = symbol_constraint.initial_price + symbol_constraint.price_step * (i + deviation_factor),
                    quantity = random.Next(1000, 10000)
                };

                bids.Add(bid);
            }

            asks = asks.OrderBy(x => Math.Round(x.price, precision)).ToList();
            bids = bids.OrderByDescending(x => x.price).ToList();

            decimal last_price = (asks.Count > 0 && bids.Count > 0) ? (random.Next(0, 2) % 2 == 0 ? asks[0].price : bids[0].price) : 0;

            return new Orderbook
            {
                symbol = symbol_constraint.symbol,
                code = symbol_constraint.code,
                asks = asks,
                bids = bids,
                last_price = last_price
            };
        }
    }
}
