import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.awt.geom.Ellipse2D;
import java.awt.geom.Line2D;
import java.awt.geom.Point2D;
import java.awt.geom.Rectangle2D;
import java.util.ArrayList;

public class JContentArea extends JPanel {

    private Tools currentTool;
    private Color currentColor = Color.GREEN;
    private int currentWidth = 2;
    private Shape currentShape;

    private NiceShape currentNiceShape;
    private NiceShape currentlySelectedNiceShape;

    private boolean moving = false;
    private double offsetX;
    private double offsetY;

    private Rectangle2D resizeBox1;
    private Rectangle2D resizeBox2;

    private boolean resizing = false;
    private Rectangle2D currentResizeBox;

    private Point2D previousPoint;

    private DisplayList displayList;
    private ArrayList<NiceOval> bubblesList = new ArrayList<>();

    private Point2D currentStartPoint;

    private int polygonSides = 3;
    private int[] currentXArray = new int[3];
    private int[] currentYArray = new int[3];
    private int currentPolyIndex = 0;
    private ArrayList<Rectangle2D> polyResizeBoxes = new ArrayList<>();
    private ArrayList<Ellipse2D> currentPolygonPoints = new ArrayList<>();
    private int polyResizeBoxIndex;
    private int currentColorNumber = 1;



    public JContentArea() {

        displayList = new DisplayList();
        addKeyListener(new KeyAdapter() {
            @Override
            public void keyTyped(KeyEvent e) {
                if (e.getExtendedKeyCode() == KeyEvent.VK_BACK_SPACE || e.getExtendedKeyCode() == KeyEvent.VK_DELETE) {
                    if (currentlySelectedNiceShape != null) {
                        displayList.removeNiceShape(currentlySelectedNiceShape);
                        resizeBox1 = null;
                        resizeBox2 = null;
                        polyResizeBoxes.clear();
                        UpdateContainedBubbles();
                        repaint();
                    }
                }
            }
        });

        double startY = 15;
        for (int i = 0; i < 30; i++) {
            double startX;
            int numBubbles;
            if (i % 2 == 1) {
                startX = 77.5;
                numBubbles = 10;
            } else {
                startX = 65;
                numBubbles = 11;
            }
            for (int j = 0; j < numBubbles; j++) {
                Ellipse2D bubble = new Ellipse2D.Double();
                bubble.setFrame(startX + j*25, startY + i*22, 25, 25);
                NiceOval niceOval = new NiceOval(bubble);
                niceOval.setXIndex(j);
                niceOval.setYIndex(i);
                bubblesList.add(niceOval);
            }
        }


        setFocusable(true);

        addMouseListener(new MouseAdapter() {
            @Override
            public void mousePressed(MouseEvent e) {
                if (!e.isPopupTrigger()) {
                    if (currentTool == Tools.POLYGON) {
                        currentXArray[currentPolyIndex] = e.getX();
                        currentYArray[currentPolyIndex] = e.getY();
                        currentPolygonPoints.add(new Ellipse2D.Double(e.getX() - 3, e.getY() - 3, 6, 6));
                        if (currentPolyIndex == polygonSides - 1) {
                            Polygon newPoly = new Polygon(currentXArray, currentYArray, polygonSides);
                            NicePolygon nicePoly = new NicePolygon(newPoly);
                            nicePoly.setColor(currentColor);
                            nicePoly.setColorNumber(currentColorNumber);
                            displayList.addNiceShape(nicePoly);
                            currentPolyIndex = 0;
                            currentXArray = new int[polygonSides];
                            currentYArray = new int[polygonSides];
                            currentPolygonPoints.clear();
                        } else {
                            currentPolyIndex++;
                        }
                    }
                    else if (currentTool == Tools.LINE) {
                        currentStartPoint = new Point2D.Double(e.getX(), e.getY());
                        currentShape = new Line2D.Double(e.getX(), e.getY(), e.getX(), e.getY());
                        currentNiceShape = new NiceLine((Line2D) currentShape);
                        displayList.addNiceShape(currentNiceShape);
                    } else if (currentTool == Tools.RECTANGLE) {
                        currentStartPoint = new Point2D.Double(e.getX(), e.getY());
                        currentShape = new Rectangle2D.Double(e.getX(), e.getY(), 1, 1);
                        currentNiceShape = new NiceRectangle((Rectangle2D) currentShape);
                        displayList.addNiceShape(currentNiceShape);
                    } else if (currentTool == Tools.OVAL) {
                        currentStartPoint = new Point2D.Double(e.getX(), e.getY());
                        currentShape = new Ellipse2D.Double(e.getX(), e.getY(), 1, 1);
                        currentNiceShape = new NiceOval((Ellipse2D) currentShape);
                        displayList.addNiceShape(currentNiceShape);
                    } else if (currentTool == Tools.SELECT) {
                        requestFocusInWindow();
                        Rectangle2D selectBox = new Rectangle2D.Double(e.getX() - 2, e.getY() - 2, 4, 4);
                        moving = false;
                        resizing = false;
                        currentResizeBox = null;
                        for (int i = 0; i < polyResizeBoxes.size(); i++) {
                            if (polyResizeBoxes.get(i).contains(e.getPoint())) {
                                resizing = true;
                                polyResizeBoxIndex = i;
                            }
                        }
                        if (resizing) {
                            currentResizeBox = polyResizeBoxes.get(polyResizeBoxIndex);
                            currentStartPoint = new Point2D.Double(currentlySelectedNiceShape.getBoundingBox().getX(),
                                    currentlySelectedNiceShape.getBoundingBox().getY());
                        }
                        else if (resizeBox1 != null && (resizeBox1.contains(e.getPoint()))) {
                            resizing = true;
                            currentStartPoint = new Point2D.Double(currentlySelectedNiceShape.getBoundingBox().getX(),
                                    currentlySelectedNiceShape.getBoundingBox().getY());
                            currentResizeBox = resizeBox1;
                        } else if (resizeBox2 != null && resizeBox2.contains(e.getPoint())) {
                            resizing = true;
                            currentResizeBox = resizeBox2;
                        } else if (currentlySelectedNiceShape != null && (currentlySelectedNiceShape.contains(selectBox)
                        || currentlySelectedNiceShape.intersects(selectBox))) {

                            ///////moving
                            moving = true;
                            offsetX = e.getX() - currentlySelectedNiceShape.getBoundingBox().getX();
                            offsetY = e.getY() - currentlySelectedNiceShape.getBoundingBox().getY();
                            previousPoint = e.getPoint();
                        } else {
                            currentlySelectedNiceShape = null;
                            ///deselect
                            resizeBox1 = null;
                            resizeBox2 = null;
                            polyResizeBoxes.clear();
                            currentResizeBox = null;

                            for (NiceShape niceShape : displayList.getNiceShapes()) {
                                if (niceShape != currentlySelectedNiceShape) {
                                    if (niceShape.intersects(selectBox)) {
                                        if (currentlySelectedNiceShape == null
                                                || niceShape.getClass() == NiceLine.class
                                                || currentlySelectedNiceShape.contains(niceShape.getBoundingBox())) {
                                            currentlySelectedNiceShape = niceShape;
                                        }
                                    }
                                }
                            }
                            if (currentlySelectedNiceShape != null) {
                                setSelected(currentlySelectedNiceShape);
                                repaint();
                            }
                        }
                    } else if (currentTool == Tools.SELECT_BUBBLE) {
                        for (NiceOval bubble : bubblesList) {
                            if (bubble.contains(e.getPoint())) {
                                if (bubble.getContained()) {
                                    bubble.setManuallyChanged(true);
                                    bubble.setContained(false);
                                    bubble.setColor(Color.BLACK);
                                    bubble.setColorNumber(0);
                                } else {
                                    bubble.setManuallyChanged(true);
                                    bubble.setContained(true);
                                    bubble.setColor(currentColor);
                                    bubble.setColorNumber(currentColorNumber);
                                }
                            }
                        }
                    }
                    if (currentNiceShape != null) {
                        currentNiceShape.setColor(currentColor);
                        currentNiceShape.setColorNumber(currentColorNumber);
                    }
                    UpdateContainedBubbles();
                    repaint();
                }
            }

            @Override
            public void mouseReleased(MouseEvent e) {
                if (resizing) {
                    resizing = false;
                    setSelected(currentlySelectedNiceShape);
                }
                if (moving) {
                    moving = false;
                }
                repaint();
            }
        });

        addMouseMotionListener(new MouseAdapter() {
            @Override
            public void mouseDragged(MouseEvent e) {
                if (!e.isPopupTrigger()) {
                    if (currentTool == Tools.LINE) {
                        ((Line2D.Double) currentShape).setLine(currentStartPoint.getX(), currentStartPoint.getY(), e.getX(), e.getY());
                    } else if (currentTool == Tools.RECTANGLE) {
                        double x = currentStartPoint.getX();
                        double y = currentStartPoint.getY();
                        double w;
                        double h;
                        if (e.getX() < x) {
                            w = x - e.getX();
                            x = e.getX();
                        } else {
                            w = e.getX() - x;
                        }
                        if (e.getY() < y) {
                            h = y - e.getY();
                            y = e.getY();
                        } else {
                            h = e.getY() - y;
                        }
                        ((Rectangle2D.Double) currentShape).setRect(x, y, w, h);
                    } else if (currentTool == Tools.OVAL) {
                        double x = currentStartPoint.getX();
                        double y = currentStartPoint.getY();
                        double w;
                        double h;
                        if (e.getX() < x) {
                            w = x - e.getX();
                            x = e.getX();
                        } else {
                            w = e.getX() - x;
                        }
                        if (e.getY() < y) {
                            h = y - e.getY();
                            y = e.getY();
                        } else {
                            h = e.getY() - y;
                        }
                        ((Ellipse2D.Double) currentShape).setFrame(x, y, w, h);
                    } else if (currentTool == Tools.SELECT) {
                        requestFocusInWindow();
                        if (moving) {
                            setSelected(currentlySelectedNiceShape);
                            currentlySelectedNiceShape.setPosition(e.getX(), e.getY(), offsetX, offsetY);
                            previousPoint = e.getPoint();
                        } else if (resizing) {
                            double x = currentStartPoint.getX();
                            double y = currentStartPoint.getY();
                            double w;
                            double h;
                            if (e.getX() < x) {
                                w = x - e.getX();
                                x = e.getX();
                            } else {
                                w = e.getX() - x;
                            }
                            if (e.getY() < y) {
                                h = y - e.getY();
                                y = e.getY();
                            } else {
                                h = e.getY() - y;
                            }
                            if (currentlySelectedNiceShape.getClass() == NiceRectangle.class) {
                                Rectangle2D rect = (Rectangle2D) currentlySelectedNiceShape.getShape();
                                rect.setRect(x, y, w, h);
                                currentResizeBox.setRect(e.getX() - 5, e.getY() - 5, 10, 10);
                            } else if (currentlySelectedNiceShape.getClass() == NiceOval.class) {
                                Ellipse2D oval = (Ellipse2D) currentlySelectedNiceShape.getShape();
                                oval.setFrame(x, y, w, h);
                                currentResizeBox.setRect(e.getX() - 5, e.getY() - 5, 10, 10);
                            } else if (currentlySelectedNiceShape.getClass() == NiceLine.class) {
                                Line2D line = (Line2D) currentlySelectedNiceShape.getShape();
                                if (currentResizeBox.contains(line.getP1())) {
                                    line.setLine(e.getX(), e.getY(), line.getX2(), line.getY2());
                                    currentResizeBox.setRect(e.getX() - 5, e.getY() - 5, 10, 10);
                                } else {
                                    line.setLine(line.getX1(), line.getY1(), e.getX(), e.getY());
                                    currentResizeBox.setRect(e.getX() - 5, e.getY() - 5, 10, 10);
                                }
                            } else if (currentlySelectedNiceShape.getClass() == NicePolygon.class) {
                                currentResizeBox.setRect(e.getX() - 5, e.getY() - 5, 10, 10);
                                ((Polygon)currentlySelectedNiceShape.getShape()).xpoints[polyResizeBoxIndex] = e.getX();
                                ((Polygon)currentlySelectedNiceShape.getShape()).ypoints[polyResizeBoxIndex] = e.getY();
                                ((Polygon)currentlySelectedNiceShape.getShape()).invalidate();
                            }
                        }
                    }
                    if (currentlySelectedNiceShape != null) {
                        currentlySelectedNiceShape.setColor(currentColor);
                        currentlySelectedNiceShape.setColorNumber(currentColorNumber);
                    }
                    if (currentNiceShape != null) {
                        currentNiceShape.setColor(currentColor);
                        currentNiceShape.setColorNumber(currentColorNumber);
                    }
                    UpdateContainedBubbles();
                    repaint();
                }
            }
        });
    }


    public void setSelected(NiceShape shape) {
        if (shape.getClass() == NiceRectangle.class || shape.getClass() == NiceOval.class) {
            Rectangle2D resizeBox = new Rectangle2D.Double();
            double x = shape.getBoundingBox().getX() + shape.getBoundingBox().getWidth();
            double y = shape.getBoundingBox().getY() + shape.getBoundingBox().getHeight();
            resizeBox.setRect(x - 5, y - 5, 10, 10);
            resizeBox1 = resizeBox;
        } else if (shape.getClass() == NiceLine.class) {
            double x1 = ((Line2D)shape.getShape()).getX1();
            double x2 = ((Line2D)shape.getShape()).getX2();
            double y1 = ((Line2D)shape.getShape()).getY1();
            double y2 = ((Line2D)shape.getShape()).getY2();
            resizeBox1 = new Rectangle2D.Double(x1 - 5, y1 - 3, 10, 10);
            resizeBox2 = new Rectangle2D.Double(x2 - 5, y2 - 3, 10, 10);
        } else if (shape.getClass() == NicePolygon.class) {
            polyResizeBoxes.clear();
            int[] boxXPoints = ((Polygon)currentlySelectedNiceShape.getShape()).xpoints;
            int[] boxYPoints = ((Polygon)currentlySelectedNiceShape.getShape()).ypoints;
            for (int i = 0; i < ((Polygon)currentlySelectedNiceShape.getShape()).npoints; i++) {
                polyResizeBoxes.add(new Rectangle2D.Double(boxXPoints[i] - 5, boxYPoints[i] - 5, 10, 10));
            }

        }
        repaint();
    }


    @Override
    protected void paintComponent(Graphics g) {
        super.paintComponent(g);


        Graphics2D g2 = (Graphics2D) g;


        g2.setStroke(new BasicStroke(1));
        for (NiceOval bubble : bubblesList) {
            if (bubble.getContained()) {
                g2.setColor(bubble.getColor());
            } else {
                g2.setColor(Color.black);
            }
            g2.draw(bubble.getShape());
        }


        g2.setStroke(new BasicStroke(2));
        g2.setColor(Color.BLACK);
        for (NiceShape niceShape : displayList.getNiceShapes()) {
//            g2.setColor(niceShape.getColor());
            //g2.setStroke(new BasicStroke(niceShape.getLineWidth()));
            g2.draw(niceShape.getShape());

        }
        g2.setColor(Color.BLACK);
        g2.setStroke(new BasicStroke(2));
        g2.setStroke(new BasicStroke(1));
        g2.setColor(Color.BLACK);
        if (resizeBox1 != null) {
            g2.draw(resizeBox1);
            g2.setColor(Color.WHITE);
            g2.fill(resizeBox1);
        }
        if (resizeBox2 != null) {
            g2.setColor(Color.BLACK);
            g2.draw(resizeBox2);
            g2.setColor(Color.WHITE);
            g2.fill(resizeBox2);
        }

        for (Rectangle2D box : polyResizeBoxes) {
            g2.setColor(Color.BLACK);
            g2.draw(box);
            g2.setColor(Color.WHITE);
            g2.fill(box);
        }

        g2.setColor(Color.BLACK);
        for (Ellipse2D polygonCircle : currentPolygonPoints) {

            g2.draw(polygonCircle);
            g2.fill(polygonCircle);
        }


    }

    public void SetTool(Tools tool) {
        currentNiceShape = null;
        currentlySelectedNiceShape = null;
        resizeBox1 = null;
        resizeBox2 = null;
        polyResizeBoxes.clear();
        currentPolygonPoints.clear();
        currentTool = tool;
        repaint();
    }

    public ArrayList<NiceOval> getBubblesList() {
        return bubblesList;
    }

    public void UpdateContainedBubbles() {
        for (NiceOval bubble : bubblesList) {
            boolean intersected = false;
            Color color = Color.BLACK;
            int colorNumber = 0;
            for (NiceShape niceShape : displayList.getNiceShapes()) {
                if (niceShape.contains(bubble.getBoundingBox()) || niceShape.intersects(bubble.getBoundingBox())) {
                    intersected = true;
                    color = niceShape.getColor();
                    colorNumber = niceShape.getColorNumber();
                }
            }
            if (intersected && !bubble.getManuallyChanged() && !bubble.getContained()) {
                bubble.setColor(color);
                bubble.setColorNumber(colorNumber);
                bubble.setContained(true);
            }
            if (!intersected && !bubble.getManuallyChanged()) {
                bubble.setColor(color.BLACK);
                bubble.setColorNumber(0);
                bubble.setContained(false);
            }
            if (bubble.getManuallyChanged() && !bubble.getContained() && !intersected) {
                bubble.setManuallyChanged(false);
            }
                if (bubble.getColor() == Color.WHITE) {
                        Color[] colorChoices = {Color.GREEN, Color.CYAN, Color.MAGENTA, Color.YELLOW, Color.RED };
                        java.util.Random rand = new java.util.Random();
                        int choice = rand.nextInt(5);
                        bubble.setColor(colorChoices[choice]);
                        bubble.setColorNumber(choice + 1);
                }
        }
    }

    public void SetPolygonSides(int sides) {
        polygonSides = sides;
        currentXArray = new int[sides];
        currentYArray = new int[sides];
    }

    public void SetBubbleColor(Color color) {
        currentColor = color;
    }
    public void SetColorNumber(int num) {
        currentColorNumber = num;
    }

}
