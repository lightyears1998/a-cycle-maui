using ACycle.Entities;
using ACycle.Models;
using ACycle.Repositories;
using System.Text;

namespace ACycle.Services
{
    public class ActivityCategoryService : EntryService<ActivityCategoryV1, ActivityCategory>, IActivityCategoryService
    {
        private readonly IEntryRepository<ActivityCategoryV1> _categoryRepository;
        private readonly Graph _graph = new();

        public ActivityCategoryService(
            IConfigurationService configurationService,
            IEntryRepository<ActivityCategoryV1> categoryRepository)
            : base(configurationService, categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public override async Task InitializeAsync()
        {
            await LoadCategoryGraphAsync();
        }

        private async Task LoadCategoryGraphAsync()
        {
            _graph.Clear();

            var categories = ConvertToModel(await _categoryRepository.FindAllAsync());
            foreach (var category in categories)
            {
                _graph.Insert(category);
            }
            foreach (var category in categories)
            {
                _graph.Patch(category);
            }
        }

        public async Task<ActivityCategory> SaveAsync(ActivityCategory category)
        {
            await base.SaveAsync(category);
            _graph.Patch(category);
            return category;
        }

        public ActivityCategory? GetParentCategory(ActivityCategory category)
        {
            return _graph.GetParent(category);
        }

        public List<ActivityCategory> GetDescendentCategories(ActivityCategory category)
        {
            return _graph.GetDescendants(category);
        }

        public class Graph
        {
            public static readonly GraphNode Root = new(null);
            private readonly Dictionary<Guid, GraphNode> _mappings = new();

            public GraphNode Insert(ActivityCategory category)
            {
                GraphNode node = new(category);
                _mappings[category.Uuid] = node;
                return node;
            }

            public GraphNode Patch(ActivityCategory category)
            {
                GraphNode node = _mappings[category.Uuid];
                GraphNode oldParent = node.Parent;
                GraphNode newParent = category.ParentUuid.HasValue ? (GetNodeByUuid(category.ParentUuid.Value) ?? Root) : Root;

                if (oldParent != newParent)
                {
                    oldParent.Descendants.Remove(node);
                    newParent.Descendants.Add(node);
                    node.Parent = newParent;
                }

                return node;
            }

            public void Clear()
            {
                _mappings.Clear();
            }

            public ActivityCategory? GetParent(ActivityCategory category)
            {
                if (!category.ParentUuid.HasValue)
                    return null;

                var node = _mappings[category.ParentUuid.Value];
                return node.Category;
            }

            public GraphNode GetNode(ActivityCategory category)
            {
                return _mappings[category.Uuid];
            }

            public GraphNode? GetNodeByUuid(Guid uuid)
            {
                if (_mappings.ContainsKey(uuid))
                    return _mappings[uuid];
                return null;
            }

            public List<GraphNode> GetNodes()
            { return _mappings.Values.ToList(); }

            public List<ActivityCategory> GetDescendants(ActivityCategory category)
            {
                var node = _mappings[category.Uuid];
                return node.Descendants.Select(descendant => descendant.Category!).ToList();
            }
        }

        public class GraphNode
        {
            public GraphNode Parent = Graph.Root;

            public List<GraphNode> Descendants = new();

            public Guid? Uuid => Category?.Uuid;

            public ActivityCategory? Category;

            public GraphNode(ActivityCategory? category)
            {
                Category = category;
            }

            public override string ToString()
            {
                Stack<GraphNode> stack = new();
                GraphNode node = this;
                while (node.Parent != Graph.Root)
                {
                    stack.Push(node);
                    node = node.Parent;
                }

                StringBuilder str = new();
                while (stack.Count > 0)
                {
                    str.Append(' ');
                    str.Append(stack.Pop().Category!.Name);
                }

                return str.ToString().Trim();
            }
        }
    }

    public interface IActivityCategoryService : IEntryService<ActivityCategoryV1, ActivityCategory>
    {
        List<ActivityCategory> GetDescendentCategories(ActivityCategory category);

        ActivityCategory? GetParentCategory(ActivityCategory category);
    }
}
