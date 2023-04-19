public class LayerMasks {
    private static int layer_ui = (1 << 5);
    private static int layer_floor = (1 << 6);
    private static int layer_object = (1 << 7);

    public static int UI { get { return layer_ui; } }
    public static int Floor { get { return layer_floor; } }
    public static int Object { get { return layer_object; } }
}