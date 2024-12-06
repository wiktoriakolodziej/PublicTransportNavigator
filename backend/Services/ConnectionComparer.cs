using PublicTransportNavigator.Dijkstra;

namespace PublicTransportNavigator.Services
{
    public class ConnectionComparer : IComparer<Connection>
    {
        public int Compare(Connection? x, Connection? y)
        {
            if(x == null && y == null) return 0;
            if(x == null) return -1;
            if(y == null) return 1;
            return x.DepartureTime
                .CompareTo(y.DepartureTime);
        }
    }
}
