import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import java.io.*;
import java.util.ArrayList;
import javax.swing.*;
import java.awt.*;
import java.net.URL;


public class Main {

    private static JButton selectedButton = null;
    private static JButton selectedColorButton = null;
    private static ArrayList<JContentArea> contentAreas = null;
    private int currentIndex;
    private JContentArea currentArea;

    public Main() {

        JFrame frame = new JFrame();
        Dimension screenDimensions = Toolkit.getDefaultToolkit().getScreenSize();
        frame.setLocation(screenDimensions.width / 2 - 250, screenDimensions.height / 2 - 400);
        frame.setSize(450,750);

        frame.setResizable(false);


        contentAreas = new ArrayList<>();
        currentArea = new JContentArea();
        contentAreas.add(currentArea);
        currentArea.setVisible(true);
        currentIndex = 0;


        currentArea.setPreferredSize(new Dimension(450, 750));
        JPanel contentAreaPanel = new JPanel();
        contentAreaPanel.add(currentArea, BorderLayout.CENTER);
        frame.add(contentAreaPanel, BorderLayout.CENTER);
        frame.setVisible(true);



        JLabel statusBar = new JLabel();
        frame.add(statusBar, BorderLayout.SOUTH);
        statusBar.setHorizontalAlignment(SwingConstants.CENTER);
        statusBar.setVisible(true);





        JToolBar toolbar = new JToolBar(JToolBar.VERTICAL);
        JPanel westPanel = new JPanel();

        westPanel.add(toolbar, BorderLayout.NORTH);
        toolbar.setFloatable(false);

        frame.add(westPanel, BorderLayout.WEST);



        frame.pack();


        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

        JMenuBar menuBar = new JMenuBar();
        frame.add(menuBar, BorderLayout.NORTH);

        JMenu fileMenu = new JMenu("File");
        JMenuItem newItem = new JMenuItem("New");
        JMenuItem saveItem = new JMenuItem("Save");
        JMenuItem quitItem = new JMenuItem("Quit");


        fileMenu.add(newItem);
        fileMenu.add(saveItem);
        fileMenu.add(quitItem);


        menuBar.add(fileMenu);




        JButton selectButton = new JButton();
        JButton lineButton = new JButton();
        JButton rectangleButton = new JButton();
        JButton ovalButton = new JButton();

        JButton polygonButton = new JButton();
        JButton selectBubbleButton = new JButton();

        JButton greenButton = new JButton();
        greenButton.setMnemonic(KeyEvent.VK_1);
        JButton magentaButton = new JButton();
        magentaButton.setMnemonic(KeyEvent.VK_2);
        JButton yellowButton = new JButton();
        yellowButton.setMnemonic(KeyEvent.VK_3);
        JButton cyanButton = new JButton();
        cyanButton.setMnemonic(KeyEvent.VK_4);
        JButton redButton = new JButton();
        redButton.setMnemonic(KeyEvent.VK_5);
        JButton randomColorButton = new JButton();


        selectedButton = selectButton;
        selectButton.setSelected(true);

        URL selectImageLocation = Main.class.getResource("select.png");
        ImageIcon selectIcon = new ImageIcon(selectImageLocation);
        selectIcon.setImage(selectIcon.getImage().getScaledInstance(20,20, Image.SCALE_SMOOTH));
        selectButton.setIcon(selectIcon);
        toolbar.add(selectButton);


        URL selectBubbleImageLocation = Main.class.getResource("selectBubble.png");
        ImageIcon selectBubbleIcon = new ImageIcon(selectBubbleImageLocation);
        selectBubbleIcon.setImage(selectBubbleIcon.getImage().getScaledInstance(20,20, Image.SCALE_SMOOTH));
        selectBubbleButton.setIcon(selectBubbleIcon);
        toolbar.add(selectBubbleButton);




        URL lineImageLocation = Main.class.getResource("lineDraw.png");
        ImageIcon lineIcon = new ImageIcon(lineImageLocation);
        lineIcon.setImage(lineIcon.getImage().getScaledInstance(20,20, Image.SCALE_SMOOTH));
        lineButton.setIcon(lineIcon);
        toolbar.add(lineButton);

        URL rectangleImageIcon = Main.class.getResource("rectangle.png");
        ImageIcon rectangleIcon = new ImageIcon(rectangleImageIcon);
        rectangleIcon.setImage(rectangleIcon.getImage().getScaledInstance(20, 20, Image.SCALE_SMOOTH));
        rectangleButton.setIcon(rectangleIcon);
        toolbar.add(rectangleButton);

        URL ovalImageIcon = Main.class.getResource("oval.png");
        ImageIcon ovalIcon = new ImageIcon(ovalImageIcon);
        ovalIcon.setImage(ovalIcon.getImage().getScaledInstance(20, 20, Image.SCALE_SMOOTH));
        ovalButton.setIcon(ovalIcon);
        toolbar.add(ovalButton);


        URL polygonImageIcon = Main.class.getResource("polygon.png");
        ImageIcon polygonIcon = new ImageIcon(polygonImageIcon);
        polygonIcon.setImage(polygonIcon.getImage().getScaledInstance(20, 20, Image.SCALE_SMOOTH));
        polygonButton.setIcon(polygonIcon);
        toolbar.add(polygonButton);


        JSpinner numChooser = new JSpinner(new SpinnerNumberModel(3.0, 3.0, 8.0, 1));
        numChooser.setVisible(true);
        numChooser.setPreferredSize(new Dimension(20, 20));
        toolbar.add(numChooser);
        numChooser.setVisible(false);
        numChooser.addChangeListener(e -> {
            currentArea.SetPolygonSides((int)Math.round((double)numChooser.getValue()));
        });


        JLabel placeHolder = new JLabel("----");
        toolbar.add(placeHolder);



        //5 color buttons, 1 rainbow / random
        //green. red. cyan. yellow. magenta. rainbow
        //add to colortoolbar
        URL greenImageIcon = Main.class.getResource("green.png");
        ImageIcon greenIcon = new ImageIcon(greenImageIcon);
        greenIcon.setImage(greenIcon.getImage().getScaledInstance(20, 20, Image.SCALE_SMOOTH));
        greenButton.setIcon(greenIcon);
        toolbar.add(greenButton);

        URL magentaImageIcon = Main.class.getResource("magenta.png");
        ImageIcon magentaIcon = new ImageIcon(magentaImageIcon);
        magentaIcon.setImage(magentaIcon.getImage().getScaledInstance(20, 20, Image.SCALE_SMOOTH));
        magentaButton.setIcon(magentaIcon);
        toolbar.add(magentaButton);

        URL yellowImageIcon = Main.class.getResource("yellow.png");
        ImageIcon yellowIcon = new ImageIcon(yellowImageIcon);
        yellowIcon.setImage(yellowIcon.getImage().getScaledInstance(20, 20, Image.SCALE_SMOOTH));
        yellowButton.setIcon(yellowIcon);
        toolbar.add(yellowButton);

        URL cyanImageIcon = Main.class.getResource("cyan.png");
        ImageIcon cyanIcon = new ImageIcon(cyanImageIcon);
        cyanIcon.setImage(cyanIcon.getImage().getScaledInstance(20, 20, Image.SCALE_SMOOTH));
        cyanButton.setIcon(cyanIcon);
        toolbar.add(cyanButton);

        URL redImageIcon = Main.class.getResource("red.png");
        ImageIcon redIcon = new ImageIcon(redImageIcon);
        redIcon.setImage(redIcon.getImage().getScaledInstance(20, 20, Image.SCALE_SMOOTH));
        redButton.setIcon(redIcon);
        toolbar.add(redButton);

        URL randomColorImageIcon = Main.class.getResource("rainbow.png");
        ImageIcon randomColorIcon = new ImageIcon(randomColorImageIcon);
        randomColorIcon.setImage(randomColorIcon.getImage().getScaledInstance(20, 20, Image.SCALE_SMOOTH));
        randomColorButton.setIcon(randomColorIcon);
        toolbar.add(randomColorButton);


        selectedColorButton = greenButton;
        greenButton.setSelected(true);


        greenButton.addActionListener(e -> {
            selectedColorButton.setSelected(false);
            selectedColorButton = greenButton;
            greenButton.setSelected(true);
            currentArea.SetBubbleColor(Color.GREEN);
            currentArea.SetColorNumber(1);
        });
        magentaButton.addActionListener(e -> {
            selectedColorButton.setSelected(false);
            selectedColorButton = magentaButton;
            magentaButton.setSelected(true);
            currentArea.SetBubbleColor(Color.magenta);
            currentArea.SetColorNumber(2);
        });
        yellowButton.addActionListener(e -> {
            selectedColorButton.setSelected(false);
            selectedColorButton = yellowButton;
            yellowButton.setSelected(true);
            currentArea.SetBubbleColor(Color.YELLOW);
            currentArea.SetColorNumber(3);
        });
        cyanButton.addActionListener(e -> {
            selectedColorButton.setSelected(false);
            selectedColorButton = cyanButton;
            cyanButton.setSelected(true);
            currentArea.SetBubbleColor(Color.cyan);
            currentArea.SetColorNumber(4);
        });
        redButton.addActionListener(e -> {
            selectedColorButton.setSelected(false);
            selectedColorButton = redButton;
            redButton.setSelected(true);
            currentArea.SetBubbleColor(Color.RED);
            currentArea.SetColorNumber(5);
        });
        randomColorButton.addActionListener(e -> {
            selectedColorButton.setSelected(false);
            selectedColorButton = randomColorButton;
            randomColorButton.setSelected(true);
            currentArea.SetBubbleColor(Color.WHITE);
            currentArea.SetColorNumber(-1);
        });



        selectButton.addActionListener(e -> {
            numChooser.setVisible(false);
            if (selectedButton != null) {
                selectedButton.setSelected(false);
            }
            selectButton.setSelected(true);
            contentAreas.get(currentIndex).SetTool(Tools.SELECT);
            selectedButton = selectButton;
        });
        selectBubbleButton.addActionListener(e -> {
            numChooser.setVisible(false);
            if (selectedButton != null) {
                selectedButton.setSelected(false);
            }
            selectBubbleButton.setSelected(true);
            contentAreas.get(currentIndex).SetTool(Tools.SELECT_BUBBLE);
            selectedButton = selectBubbleButton;
        });
        lineButton.addActionListener(e -> {
            numChooser.setVisible(false);
            if (selectedButton != null) {
                selectedButton.setSelected(false);
            }
            lineButton.setSelected(true);
            contentAreas.get(currentIndex).SetTool(Tools.LINE);
            selectedButton = lineButton;
        });
        rectangleButton.addActionListener(e -> {
            numChooser.setVisible(false);
            if (selectedButton != null) {
                selectedButton.setSelected(false);
            }
            rectangleButton.setSelected(true);
            contentAreas.get(currentIndex).SetTool(Tools.RECTANGLE);
            selectedButton = rectangleButton;
        });
        ovalButton.addActionListener(e -> {
            numChooser.setVisible(false);
            ovalButton.setSelected(true);
            if (selectedButton != null) {
                selectedButton.setSelected(false);
            }
            ovalButton.setSelected(true);
            contentAreas.get(currentIndex).SetTool(Tools.OVAL);
            selectedButton = ovalButton;
        });
        polygonButton.addActionListener(e -> {
            numChooser.setVisible(true);

            if (selectedButton != null) {
                selectedButton.setSelected(false);
            }
            polygonButton.setSelected(true);
            contentAreas.get(currentIndex).SetTool(Tools.POLYGON);
            selectedButton = polygonButton;
        });


        quitItem.addActionListener(e -> {
            frame.dispose();
            System.exit(0);
        });

        saveItem.addActionListener(e -> {
            JFileChooser jF = new JFileChooser();
            jF.setCurrentDirectory(new File(System.getProperty("user.dir")));
            jF.showSaveDialog(frame);
            if (jF.getSelectedFile() != null) {
                File levelFile = jF.getSelectedFile();
                try {
                    if (!levelFile.toString().endsWith(".txt")) {
                        String nm = levelFile.getName();
                        levelFile = new File(nm += ".txt");
                    }
                    FileWriter w = new FileWriter(levelFile);
                    w.write(getLevelString());
                    w.close();
                    statusBar.setText("saved successfully");
                }
                catch (IOException exc) {
                    statusBar.setText("save failed");
                    return;
                }

            }

        });

        newItem.addActionListener(e -> {
            JContentArea newArea = new JContentArea();
            newArea.setPreferredSize(contentAreaPanel.getPreferredSize());
            contentAreaPanel.remove(currentArea);
            contentAreas.remove(currentArea);
            contentAreas.add(newArea);
            contentAreaPanel.add(newArea, BorderLayout.CENTER);
            contentAreaPanel.revalidate();
            contentAreaPanel.repaint();
            newArea.setVisible(true);
            selectedButton.setSelected(false);
            selectedButton = selectButton;
            selectButton.setSelected(true);
            newArea.SetTool(Tools.SELECT);
            currentArea = contentAreas.get(currentIndex);
        });

    }


    public String getLevelString() {
        int[][] bubbleArray = new int[30][11];
        for (NiceOval bubble : currentArea.getBubblesList()) {
            if (bubble.getContained()) {
                bubbleArray[bubble.getYIndex()][bubble.getXIndex()] = bubble.getColorNumber();
            }
        }
        String levelString = "MODE 0\nSIZE 11/30\nLIMIT 0/30\nCOLOR LIMIT 5\nSTARS 500/1000/1500\n";
        for (int i = 0; i < 30; i++) {
            for (int j = 0; j < 11; j++) {
                levelString += String.valueOf(bubbleArray[i][j]);
                levelString += " ";
            }
            levelString += "\n";
        }
        return levelString;
    }


//    public void SetColor(Color color) {
//        System.out.println(color);
//    }


    public static void main(String[] args) {
        SwingUtilities.invokeLater(() -> new Main());

    }
}
