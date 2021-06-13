using AutoMapper;
using KanbanBoard.DAL.EfDbContext.Converters;
using KanbanBoard.DAL.EfDbContext.DTO;
using KanbanBoard.Models;

namespace KanbanBoard.DAL.EfDbContext
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DbProject, Project>();
            CreateMap<Project, DbProject>()
                .ConvertUsing<ProjectConverter>();

            CreateMap<DbTable, Table>()
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.Project.Id));
            CreateMap<Table, DbTable>()
                .ConvertUsing<TableConverter>();

            CreateMap<DbColumn, Column>()
                .ForMember(dest => dest.TableId, opt => opt.MapFrom(src => src.Table.Id));
            CreateMap<Column, DbColumn>()
                .ConvertUsing<ColumnConverter>();

            CreateMap<DbTask, Task>()
                .ForMember(dest => dest.ColumnId, opt => opt.MapFrom(src => src.Column.Id));
            CreateMap<Task, DbTask>()
                .ConvertUsing<TaskConverter>();

            CreateMap<DbComment, Comment>()
            .ForMember(dest => dest.TaskId, opt => opt.MapFrom(src => src.Task.Id));
            CreateMap<Comment, DbComment>()
                .ConvertUsing<CommentConverter>();

            CreateMap<DbUser, User>()
                .ForMember(dest => dest.Id, src => src.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, src => src.MapFrom(src => src.Name))
                .ForAllOtherMembers(x => x.Ignore());
            CreateMap<User, DbUser>()
                .ConvertUsing<UserConverter>();
        }
    }
}
