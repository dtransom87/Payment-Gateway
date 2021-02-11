using System;
using System.Linq;
using System.Threading.Tasks;
using Marten;
using payment_gateway.Models;
using payment_gateway.Services;

namespace payment_gateway.Repositories
{
    public class PaymentRepository : IRepository<Payment>
    {
        private IDocumentStore _store;

        public PaymentRepository(IDocumentStore store)
        {
            _store = store;
        }


        public async Task<Payment> Create(Payment payment)
        {
            using (var session = _store.LightweightSession())
            {
                payment.Id = Guid.NewGuid().ToString();
                session.Insert(payment);
                await session.SaveChangesAsync();
            }

            return payment;
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Payment> Get(string id)
        {
            using (var session = _store.QuerySession())
            {
                var existing = await session
                    .Query<Payment>()
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                return existing;    
            }
        }

        public async Task<Payment> Update(Payment payment)
        {
            using (var session = _store.LightweightSession())
            {
                session.Update(payment);
                await session.SaveChangesAsync();
            }

            return payment;
        }
    }
}