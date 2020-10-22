import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.awt.geom.Ellipse2D;
import java.awt.geom.Line2D;
import java.awt.geom.Point2D;
import java.awt.geom.Rectangle2D;
import java.util.ArrayList;
import java.util.Collections;

public class NiceLine extends NiceShape {

    private Line2D line;

    public NiceLine(Line2D line) {
        this.line = line;
    }

    @Override
    public Line2D getShape() {
        return line;
    }

    @Override
    public boolean contains(Rectangle2D rect) {
        return line.contains(rect);
    }

    @Override
    public boolean intersects(Rectangle2D rect) {
        return line.intersects(rect);
    }

    @Override
    public Rectangle2D getBoundingBox() {
        return line.getBounds();
    }

    @Override
    public void setPosition(double x, double y, double mouseOffsetX, double mouseOffsetY) {

        double diffX = line.getX2() - line.getX1();
        double diffY = line.getY2() - line.getY1();

        double x1 = x + (line.getX1() - line.getBounds().getX()) - mouseOffsetX;
        double y1 = y + (line.getY1() - line.getBounds().getY()) - mouseOffsetY;

        double x2 = x1 + diffX;
        double y2 = y1 + diffY;

        line.setLine(x1, y1, x2, y2);

    }
}
