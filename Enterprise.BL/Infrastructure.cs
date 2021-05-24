using AutoMapper.QueryableExtensions;
using Enterprise.BL.Data;
using Enterprise.BL.ViewModels;
using Enterprise.Infrastrucre.General;
using Enterprise.Infrastrucre.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Enterprise.Infrastrucre.General
{
    /*
     * Consumer defines the interface(protocol).
     * Implementation layer depends on the consumer.
     * Consumer does not depend on implementation.
     */
    interface IModelConverter<TModel, TPersistence>
    {
        TModel ToModel(TPersistence persisted);
        TPersistence ToPersisted(TModel model);
        void CopyChanges(TModel from, TPersistence to);
    }
    class MappingRepository<TModel, TPersistence, TDbContext> : IRepository<TModel> where TPersistence : class where TDbContext : DbContext
    {
        private TDbContext DbContext { get; }
        private IModelConverter<TModel, TPersistence> Converter { get; }
        private Func<TDbContext, DbSet<TPersistence>> GetDbSet { get; }
        private IDictionary<TModel, TPersistence> MaterilizedObjects { get; }
        private DbSet<TPersistence> DbSet => this.GetDbSet(this.DbContext);
        private Action EnsureNotDisposed { get; set; } = () => { };
        public MappingRepository(Func<TDbContext> dbContextFactory, IModelConverter<TModel, TPersistence> converter, Func<TDbContext, DbSet<TPersistence>> getDbSet)
        {
            this.DbContext = dbContextFactory();
            this.Converter = converter;
            this.MaterilizedObjects = new Dictionary<TModel, TPersistence>();
            this.GetDbSet = getDbSet;
        }
        public void Add(TModel obj)
        {
            this.EnsureNotDisposed();
            if (obj == null) throw new ArgumentNullException();
            TPersistence persisted = this.Converter.ToPersisted(obj);
            this.DbSet.Add(persisted);
            this.MaterilizedObjects[obj] = persisted;
        }

        public void Delete(TModel obj)
        {
            this.EnsureNotDisposed();
            if (obj == null || !this.MaterilizedObjects.ContainsKey(obj)) throw new ArgumentNullException();
            TPersistence persisted = this.MaterilizedObjects[obj];
            this.DbSet.Remove(persisted);
            this.MaterilizedObjects.Remove(obj);
        }

        public void Dispose()
        {

        }

        public TModel Find(int id)
        {
            this.EnsureNotDisposed();
            TPersistence persisted = this.DbSet.Find(id);
            TModel model = this.Converter.ToModel(persisted);
            this.MaterilizedObjects[model] = persisted;
            return model;
        }

        public IEnumerable<TModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            this.EnsureNotDisposed();
            foreach (var keyValuePair in this.MaterilizedObjects)
            {
                this.Converter.CopyChanges(keyValuePair.Key, keyValuePair.Value);
            }
            this.DbContext.SaveChanges();
        }
        private bool IsDisposed { get; set; } = false;
    }
    /*
     * Domain layer defines separate domain and view models.
     * Persistence layer defines its own persistence models. 
    */
    class ReadOnlyRepository<TModel, TPersistence, TDbContext> : IReadOnlyRepository<TModel> where TPersistence : PersistenObject where TDbContext : DbContext
    {
        private TDbContext DbContext { get; }
        private Func<TDbContext, DbSet<TPersistence>> GetDbSet { get; }
        private IQueryable<TPersistence> NonTrackingQuery => this.GetDbSet(this.DbContext).AsNoTracking<TPersistence>();
        // private IQueryable<TModel> NonTrackingModelQuery => this.NonTrackingQuery.ProjectTo<TModel>(null, null, null);
        private Action EnusureNotDisposed { get; set; } = () => { };
        public ReadOnlyRepository(Func<TDbContext> dbContextFactory, Func<TDbContext, DbSet<TPersistence>> getDbSet)
        {
            this.DbContext = dbContextFactory();
            this.GetDbSet = getDbSet;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public TModel Find(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TModel> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}

namespace Enterprise.Infrastrucre
{
    static class ReadOnlyRepositories
    {
        static ReadOnlyRepositories() { }
        public static IReadOnlyRepository<ProfessorViewModel> CreateProfessorRepository =>
            new ReadOnlyRepository<ProfessorViewModel, Professor, CollegeModel>(() => new CollegeModel(), dbContext => dbContext.Professors);
    }
    class Repositories
    {
        public Repositories()
        {

        }
    }
}

namespace Enterprise.Infrastrucre.Models
{
    class CollegeModel : DbContext
    {
        public DbSet<Professor> Professors { get; set; }
    }
    class PersistenObject
    {
        [Key]
        public int Id { get; set; }
    }
    class Professor : PersistenObject
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Lastname { get; set; }
        public string LastNameInitial { get; set; }
    }
}
namespace Enterprise.Infrastrucre.Converters
{
    class ProfessorConverter : IModelConverter<Enterprise.BL.Professor, Models.Professor>
    {
        private IEnumerable<string> lastNamePatterns => new List<string> { };
        public void CopyChanges(Enterprise.BL.Professor from, Models.Professor to)
        {
            to.FirstName = from.Name.FirstName;
            to.MiddleName = from.Name.MiddleName;
            to.Lastname = from.Name.LastName;
            to.LastNameInitial = from.Name.RefactoredLastNameInitial;
        }

        public Enterprise.BL.Professor ToModel(Models.Professor persisted) => new BL.Professor(new BL.PersonalName(this.lastNamePatterns, persisted.FirstName, persisted.MiddleName, persisted.Lastname));

        public Models.Professor ToPersisted(Enterprise.BL.Professor model) => new Professor
        {
            FirstName = model.Name.FirstName,
            MiddleName = model.Name.MiddleName,
            Lastname = model.Name.LastName,
            LastNameInitial = model.Name.RefactoredLastNameInitial
        };
    }
}