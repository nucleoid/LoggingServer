using AutoMapper;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;

namespace LoggingServer.Server.Automapper
{
    public class FilterResolver : ValueResolver<IFilterModel, SearchFilter>
    {
        private readonly IReadableRepository<SearchFilter> _filterRepository;

        public FilterResolver(IReadableRepository<SearchFilter> filterRepository)
        {
            _filterRepository = filterRepository;
        }

        protected override SearchFilter ResolveCore(IFilterModel source)
        {
            return _filterRepository.Get(source.FilterId);
        }
    }
}
