using SimpleMotions.Internal;

namespace SimpleMotions {

    public interface IColorPickerViewModel {
        void SetColorToEntity(Color color);
    }

    public class ColorPickerViewModel : IColorPickerViewModel {
        
        private readonly IColorPickerModel _colorPickerModel;
        private readonly IEntitySelector _entitySelector;

        public ColorPickerViewModel(IEntitySelector entitySelector, IColorPickerModel colorPickerModel) {
            _colorPickerModel = colorPickerModel;
            _entitySelector = entitySelector;
        }

        public void SetColorToEntity(Color color) {
            var selectedEntity = _entitySelector.SelectedEntity;

            if (selectedEntity.Id == Entity.InvalidEntity.Id) {
                return;
            }
            
            _colorPickerModel.SetColorToEntity(selectedEntity, color);
        }

    }
}