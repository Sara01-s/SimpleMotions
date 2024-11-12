using SimpleMotions.Internal;

namespace SimpleMotions {

    public interface IColorPickerViewModel {
        void SetColorToEntity((float r, float g, float b, float a) color);
    }

    public class ColorPickerViewModel : IColorPickerViewModel {
        
        private readonly SmParser _smParser;
        private readonly EntitySelector _entitySelector;
        private readonly IColorPickerModel _colorPickerModel;

        private Entity _selectedEntity;
        private Color _color;

        public ColorPickerViewModel(SmParser smParser, EntitySelector entitySelector, IColorPickerViewModel colorPickerModel) {
            _smParser = smParser;
            _entitySelector = entitySelector;

            _entitySelector.OnEntitySelected.Subscribe(GetSelectedEntity);

        }

        ~ColorPickerViewModel() {
            _entitySelector.OnEntitySelected.Dispose();
        }

        public void GetSelectedEntity(Entity entity) {
            _selectedEntity = entity;
        }

        public void SetColorToEntity((float r, float g, float b, float a) color) {
            _color = _smParser.ConstructSmColor((color.r, color.g, color.b, color.a));

            _colorPickerModel.SetColorToEntity(_selectedEntity, _color);
        }

    }
}