//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using FluentNHibernate.Cfg;
//using FluentNHibernate.Mapping;
//using LearnDash.Dal.NHibernate;
//using NUnit.Framework;

//namespace LearnDashDal.Tests
//{

//    public class NHibernateTestEntity
//    {
//        public virtual int ID { get; set; }
//        public virtual string String { get; set; }
//    }

//    public class NHibernateTestEntityMap : ClassMap<NHibernateTestEntity>
//    {
//        public NHibernateTestEntityMap()
//        {
//            Id(x => x.ID);
//            Map(x => x.String);
//        }
//    }


//    public class TestQueryObject : IQueryObject
//    {

//        public string Query
//        {
//            get
//            {
//                return "from NHibernateTestEntity k where k.String = 'test'";
//            }
//        }
//    }

//    [TestFixture]
//    class RepositoryTests
//    {
//        protected SqlLiteTestDBAccess _database;


//        [SetUp]
//        public void SetUp()
//        {
//            //Adding new mapping for test entity
//            SqlLiteTestDBAccess.Configuration = Fluently.Configure(SqlLiteTestDBAccess.Configuration)
//                .Mappings(x => x.FluentMappings.Add<NHibernateTestEntityMap>()).BuildConfiguration();
//            _database = new SqlLiteTestDBAccess();
//        }

//        [Test]
//        public void Can_add_test_entity()
//        {
//            #region Arrange
//            var addedEntity = new NHibernateTestEntity() { String = "test" };

//            #endregion
//            new Repository<NHibernateTestEntity>().Add(addedEntity);
//            #region Act
//            #endregion

//            #region Assert
//            NHibernateTestEntity returnedEntity;
//            using (var session = DataAccess.OpenSession())
//            {
//                returnedEntity = session.Get<NHibernateTestEntity>(1);
//            }
//            Assert.That(returnedEntity, Is.Not.Null);
//            Assert.That(returnedEntity.String, Is.EqualTo("test"));
//            #endregion
//        }

//        [Test]
//        public void Can_get_by_id_entity()
//        {
//            #region Arrange
//            var addedEntity = new NHibernateTestEntity() { String = "test" };
//            new Repository<NHibernateTestEntity>().Add(addedEntity);
//            #endregion

//            #region Act
//            var returnedEntity = new Repository<NHibernateTestEntity>().GetById(1);
//            #endregion

//            #region Assert
//            Assert.That(returnedEntity, Is.Not.Null);
//            Assert.That(returnedEntity.String, Is.EqualTo("test"));
//            #endregion
//        }

//        [Test]
//        public void Getting_by_wrong_ID_returns_null()
//        {
//            #region Act
//            var returnedEntity = new Repository<NHibernateTestEntity>().GetById(-1);
//            #endregion

//            #region Assert
//            Assert.That(returnedEntity, Is.Null);
//            #endregion
//        }


//        [Test]
//        public void Can_Remove_Entity()
//        {
//            #region Arrange
//            var addedEntity = new NHibernateTestEntity() { String = "test" };
//            #endregion

//            #region Act
//            new Repository<NHibernateTestEntity>().Add(addedEntity);
//            new Repository<NHibernateTestEntity>().Remove(addedEntity);
//            #endregion

//            #region Assert
//            Assert.That(new Repository<NHibernateTestEntity>().GetCount(), Is.EqualTo(0));
//            #endregion
//        }


//        [Test]
//        public void Can_Update_Entity()
//        {
//            #region Arrange
//            var addedEntity = new NHibernateTestEntity() { String = "test" };
//            new Repository<NHibernateTestEntity>().Add(addedEntity);
//            #endregion

//            #region Act
//            addedEntity.String = "testUpdated";
//            new Repository<NHibernateTestEntity>().Update(addedEntity);
//            var returnedEntity = new Repository<NHibernateTestEntity>().GetById(1);
//            #endregion

//            #region Assert
//            Assert.That(returnedEntity, Is.Not.Null);
//            Assert.That(returnedEntity.String, Is.EqualTo("testUpdated"));
//            #endregion
//        }

//        [Test]
//        public void Can_Get_All_Entities()
//        {
//            #region Arrange
//            var addedEntity = new NHibernateTestEntity() { String = "test" };
//            var addedEntity1 = new NHibernateTestEntity() { String = "test1" };

//            new Repository<NHibernateTestEntity>().Add(addedEntity);
//            new Repository<NHibernateTestEntity>().Add(addedEntity1);
//            #endregion

//            #region Act

//            var entities = new Repository<NHibernateTestEntity>().GetAll();

//            #endregion

//            #region Assert
//            Assert.That(entities.Count, Is.EqualTo(2));
//            #endregion
//        }


//        [Test]
//        public void Can_Get_By_Parameter_Equals_Filter()
//        {
//            #region Arrange
//            var addedEntity = new NHibernateTestEntity() { String = "test" };
//            var addedEntity1 = new NHibernateTestEntity() { String = "test1" };

//            new Repository<NHibernateTestEntity>().Add(addedEntity);
//            new Repository<NHibernateTestEntity>().Add(addedEntity1);
//            #endregion

//            #region Act

//            var entities = new Repository<NHibernateTestEntity>().GetByParameterEqualsFilter("String", "test");

//            #endregion

//            #region Assert
//            Assert.That(entities.Count, Is.EqualTo(1));
//            #endregion
//        }

//        [Test]
//        public void Can_Get_By_Query_Filter()
//        {
//            #region Arrange
//            var addedEntity = new NHibernateTestEntity() { String = "test" };
//            var addedEntity1 = new NHibernateTestEntity() { String = "test1" };

//            new Repository<NHibernateTestEntity>().Add(addedEntity);
//            new Repository<NHibernateTestEntity>().Add(addedEntity1);
//            #endregion

//            #region Act

//            var entities = new Repository<NHibernateTestEntity>()
//                .GetByQuery("from NHibernateTestEntity k where k.String = 'test'");

//            #endregion

//            #region Assert
//            Assert.That(entities.Count, Is.EqualTo(1));
//            #endregion
//        }


//        [Test]
//        public void Can_Get_By_Query_Object()
//        {
//            #region Arrange
//            var addedEntity = new NHibernateTestEntity() { String = "test" };
//            var addedEntity1 = new NHibernateTestEntity() { String = "test1" };

//            new Repository<NHibernateTestEntity>().Add(addedEntity);
//            new Repository<NHibernateTestEntity>().Add(addedEntity1);
//            #endregion

//            #region Act

//            var entities = new Repository<NHibernateTestEntity>().GetByQueryObject(new TestQueryObject());

//            #endregion

//            #region Assert
//            Assert.That(entities.Count, Is.EqualTo(1));
//            #endregion
//        }




//        [TearDown]
//        public void TearDown()
//        {
//            _database.Dispose();
//        }



//    }
//}
