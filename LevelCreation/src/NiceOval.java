import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.awt.geom.Ellipse2D;
import java.awt.geom.Line2D;
import java.awt.geom.Point2D;
import java.awt.geom.Rectangle2D;
import java.util.ArrayList;
import java.util.Collections;


public class NiceOval extends NiceShape {

    private Ellipse2D oval;
    private String text = "";
    private int xIndex = 0;
    private int yIndex = 0;
    private boolean contained = false;
    private boolean manuallyChanged = false;

    public NiceOval(Ellipse2D oval) {
        this.oval = oval;
    }

    @Override
    public Shape getShape() {
        return oval;
    }

    @Override
    public boolean contains(Rectangle2D rect) {
        return oval.contains(rect);
    }

    public boolean contains(Point2D point) {
        return oval.contains(point);
    }

    @Override
    public boolean intersects(Rectangle2D rect) {
        return oval.intersects(rect);
    }

    @Override
    public Rectangle2D getBoundingBox() {
        return oval.getBounds();
    }

    @Override
    public void setPosition(double x, double y, double mouseOffsetX, double mouseOffsetY) {
        oval.setFrame(x - mouseOffsetX, y - mouseOffsetY, oval.getWidth(), oval.getHeight());
    }

    public void setXIndex(int index) {
        xIndex = index;
    }
    public void setYIndex(int index) {
        yIndex = index;
    }
    public int getXIndex() {
        return xIndex;
    }
    public int getYIndex() {
        return yIndex;
    }
    public void setContained(boolean contained) {
        this.contained = contained;
    }
    public boolean getContained() {
        return contained;
    }
    public void setManuallyChanged(boolean manuallyChanged) {
        this.manuallyChanged = manuallyChanged;
    }
    public boolean getManuallyChanged() {
        return manuallyChanged;
    }

}
