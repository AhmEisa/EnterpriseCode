using Moq;
using NBehave.Spec.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Enterprise.UnitTest
{
    public class Specification : SpecBase
    {

    }

    public class when_working_with_the_item_type_repository : Specification
    {

    }

    public class and_saving_a_valid_item_type : when_working_with_the_item_type_repository
    {
        private int _result;
        private IItemTypeRepository _itemTypeRepository;
        private ItemType _testItemType;
        private int _itemTypeId;

        protected override void Establish_context()
        {
            base.Establish_context();

            _itemTypeId = new Random().Next(32000);

            _itemTypeRepository = new Mock<IItemTypeRepository>().Object;
        }
        protected override void Because_of()
        {
            _result = _itemTypeRepository.Save(_testItemType);
        }


        [Fact]
        public void then_a_valid_item_type_id_should_be_returned()
        {
            _result.ShouldEqual(_itemTypeId);
        }
    }

    #region Core Business
    #region Core.Entities
    public class ItemType { public int Id { get; set; } }

    #endregion

    #region Core.Persistence
    public interface IItemTypeRepository { int Save(ItemType itemType); }

    #endregion

  
    #endregion
}
