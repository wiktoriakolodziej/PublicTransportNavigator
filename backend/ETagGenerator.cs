using PublicTransportNavigator.Models;
using System.Security.Cryptography;
using System.Text;

namespace PublicTransportNavigator
{
    public class ETagGenerator<T>(PublicTransportNavigatorContext context) where T :BaseEntity
    {
        private readonly PublicTransportNavigatorContext _context = context;
        public string GenerateEtag(long id)
        {
            var entity = _context.FindAsync<T>(id).Result;
            if (entity == null)
                throw new KeyNotFoundException(
                    $"Entity of id: {id} and type: {typeof(T)} was not found in the database");
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(entity!.LastModified.ToString()));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }

    }
}