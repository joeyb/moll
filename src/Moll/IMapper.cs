
namespace Moll
{
    public interface IMapper<in TSrc, out TDest> where TDest : new()
    {
        TDest Map(TSrc src);
    }
}
