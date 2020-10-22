import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.awt.geom.Ellipse2D;
import java.awt.geom.Line2D;
import java.awt.geom.Point2D;
import java.awt.geom.Rectangle2D;
import java.util.ArrayList;
import java.util.Collections;


public abstract class NiceShape {

    private Color color = Color.black;
    private int lineWidth = 2;


    public abstract boolean intersects(Rectangle2D rect);
    public abstract boolean contains(Rectangle2D rect);
    public abstract Rectangle2D getBoundingBox();
    public abstract void setPosition(double x, double y, double mouseOffsetX, double mouseOffsetY);
    public abstract Shape getShape();
    private int colorNumber = 1;

    public Color getColor() {
        return color;
    }
    public int getLineWidth() {
        return lineWidth;
    }
    public void setColor(Color color) {
        this.color = color;
    }
    public void setLineWidth(int width) {
        lineWidth = width;
    }

    public void setColorNumber(int colorNumber) {
        this.colorNumber = colorNumber;
    }
    public int getColorNumber() {
        return colorNumber;
    }

}
