import java.awt.*;
import java.awt.geom.Rectangle2D;

public class NicePolygon extends NiceShape {


    private Polygon polygon;

    public NicePolygon(Polygon polygon) { this.polygon = polygon; }

    @Override
    public boolean intersects(Rectangle2D rect) {
        return polygon.intersects(rect);
    }

    @Override
    public boolean contains(Rectangle2D rect) {
        return polygon.contains(rect);
    }

    @Override
    public Rectangle2D getBoundingBox() {
        return polygon.getBounds();
    }

    @Override
    public void setPosition(double x, double y, double mouseOffsetX, double mouseOffsetY) {
        double deltaX = -(polygon.getBounds().x - (x - mouseOffsetX));
        double deltaY = -(polygon.getBounds().y - (y - mouseOffsetY));
        polygon.translate(Math.round((float)deltaX), Math.round((float)deltaY));
    }

    @Override
    public Shape getShape() {
        return polygon;
    }
}
