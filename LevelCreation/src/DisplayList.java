import java.awt.geom.Line2D;
import java.util.ArrayList;

public class DisplayList {

    private ArrayList<NiceShape> niceShapes = new ArrayList<>();
    private NiceShape currentNiceShape;
    private ArrayList<Line2D> gesture = new ArrayList<>();


    public ArrayList<Line2D> getGesture() { return gesture; }

    public DisplayList() {
    }

    public void addNiceShape(NiceShape shape) {
        niceShapes.add(shape);
        currentNiceShape = shape;
    }
    public ArrayList<NiceShape> getNiceShapes() {
        return (ArrayList<NiceShape>) niceShapes.clone();
    }

    public void removeNiceShape(int shapeIndex) {
        niceShapes.remove(shapeIndex);
    }

    public void removeNiceShape(NiceShape shape) {
        niceShapes.remove(shape);
    }

}
