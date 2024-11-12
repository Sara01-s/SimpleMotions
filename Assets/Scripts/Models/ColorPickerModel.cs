
namespace SimpleMotions.Internal {

    public interface IColorPickerModel {
        void SetColorToEntity(Entity entity, Color color);
    }

    public class ColorPickerModel : IColorPickerModel {

        private readonly ComponentStorage _componentStorage;

        public ColorPickerModel(ComponentStorage componentStorage) {
            _componentStorage = componentStorage;
        }

        public  void SetColorToEntity(Entity entity, Color color) {
            _componentStorage.GetComponent<Shape>(entity.Id).Color = color;
        }

    }
}