import java.awt.*;
import java.awt.geom.Ellipse2D;
import java.awt.geom.Rectangle2D;

public class NiceRectangle extends NiceShape {

    private Rectangle2D rectangle;
    private String text = "";

    public NiceRectangle(Rectangle2D rect) {
        rectangle = rect;
    }

    public void addText(String text) {
        this.text += text;
    }


    public String getText() {
        return text;
    }

    @Override
    public boolean contains(Rectangle2D rect) {
        return rectangle.contains(rect);
    }

    @Override
    public boolean intersects(Rectangle2D rect) {
        return rectangle.intersects(rect);
    }

    @Override
    public Rectangle2D getBoundingBox() {
        return rectangle.getBounds();
    }

    @Override
    public void setPosition(double x, double y, double mouseOffsetX, double mouseOffsetY) {
        rectangle.setRect(x - mouseOffsetX, y - mouseOffsetY, rectangle.getWidth(), rectangle.getHeight());
    }

    @Override
    public Shape getShape() {
        return rectangle;
    }
}
