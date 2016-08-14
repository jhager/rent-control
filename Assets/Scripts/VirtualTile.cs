using UnityEngine;
using System.Collections;

/**
 * This tile represents both the card that is in the player's hand
 * and the tile on the table.  
 */
public class VirtualTile  {
	
	public enum Orientation {Up = 0, Clockwise90 = 1, UpsideDown = 2, CounterClockwise90 = 3};

	public static Color red = new Color(0.87f, 0.36f, 0.26f, 0.9f);
	public static Color orange = new Color(0.98f, 0.62f, 0.15F, 0.9f);
	public static Color yellow = new Color (0.91f, 0.86f, 0.29f, 0.9f);
	public static Color green = new Color(0.34f, 0.64f, 0.34f, 0.9f);
	public static Color blue = new Color(0.27f, 0.54f, 0.86f, 0.9f);
	public static Color violet = new Color (0.65F, 0.38f, 0.70f, 0.9f);
	public static Color colorless = new Color(0.87f, 0.89f, 0.91f, 0.9f);


	static ushort ZERO = 0;

	public static ushort SQUARE11 = 1;
	public static ushort SQUARE12 = 2;
	public static ushort SQUARE13 = 4;
	public static ushort SQUARE14 = 8;

	public static ushort SQUARE21 = 16;
	public static ushort SQUARE22 = 32;
	public static ushort SQUARE23 = 64;
	public static ushort SQUARE24 = 128;

	public static ushort SQUARE31 = 256;
	public static ushort SQUARE32 = 512;
	public static ushort SQUARE33 = 1024;
	public static ushort SQUARE34 = 2048;

	public static ushort SQUARE41 = 4096;
	public static ushort SQUARE42 = 8192;
	public static ushort SQUARE43 = 16384;
	public static ushort SQUARE44 = 32768;

	// x1 x2 x4 x8
	// x16 x32 x64 x128 
	// x256 x512 x1024
	// x2048 x4096 x8192 x16,386 x32,772

	//bit mask unsigned 16-bit integer to keep track of tile or board state. 
	//the grid is laid out using the statics above as follows for each primary color
	//component.
	//11 12 13 14       
	//21 22 23 24       
	//31 32 33 34 
	//41 42 43 44
	ushort redComponent = 0;
	ushort yellowComponent = 0;
	ushort blueComponent = 0;

	//left in case 4 size pieces make the game not fun to play.  There is a 50/50 chance of the 
	//size being 4 right now, since it is equal distribution of all possible playing pieces.  
	//private static int NUMBER_OF_PIECES_IN_LIBRARY = 27;
	private static int NUMBER_OF_PIECES_IN_LIBRARY = 53;
	private static ushort[] pieceLibrary = new ushort[NUMBER_OF_PIECES_IN_LIBRARY];
	static VirtualTile() {
		int i = 0;
		pieceLibrary[i++] = (ushort) (SQUARE11);
		pieceLibrary[i++] = (ushort) (SQUARE12);
		pieceLibrary[i++] = (ushort) (SQUARE13);
		pieceLibrary[i++] = (ushort) (SQUARE14);
		pieceLibrary[i++] = (ushort) (SQUARE22);

		//2 square pieces
		pieceLibrary[i++] = (ushort) (SQUARE11 | SQUARE12);
		pieceLibrary[i++] = (ushort) (SQUARE12 | SQUARE13);
		pieceLibrary[i++] = (ushort) (SQUARE13 | SQUARE14);
		pieceLibrary[i++] = (ushort) (SQUARE21 | SQUARE22);
		pieceLibrary[i++] = (ushort) (SQUARE22 | SQUARE23);
		pieceLibrary[i++] = (ushort) (SQUARE23 | SQUARE24);

		//3 square pieces
		pieceLibrary[i++] = (ushort) (SQUARE11 | SQUARE12 | SQUARE13);
		pieceLibrary[i++] = (ushort) (SQUARE12 | SQUARE13 | SQUARE14);
		pieceLibrary[i++] = (ushort) (SQUARE21 | SQUARE22 | SQUARE23);
		pieceLibrary[i++] = (ushort) (SQUARE22 | SQUARE23 | SQUARE24);
		pieceLibrary[i++] = (ushort) (SQUARE11 | SQUARE12 | SQUARE22);
		pieceLibrary[i++] = (ushort) (SQUARE12 | SQUARE13 | SQUARE23);
		pieceLibrary[i++] = (ushort) (SQUARE13 | SQUARE14 | SQUARE24);
		pieceLibrary[i++] = (ushort) (SQUARE12 | SQUARE13 | SQUARE22);
		pieceLibrary[i++] = (ushort) (SQUARE12 | SQUARE21 | SQUARE22);
		pieceLibrary[i++] = (ushort) (SQUARE12 | SQUARE22 | SQUARE23);
		pieceLibrary[i++] = (ushort) (SQUARE13 | SQUARE22 | SQUARE23);
		 
		//straight line and square
		pieceLibrary[i++] = (ushort) (SQUARE11 | SQUARE12 | SQUARE13 | SQUARE14);
		pieceLibrary[i++] = (ushort) (SQUARE21 | SQUARE22 | SQUARE23 | SQUARE24);
		pieceLibrary[i++] = (ushort) (SQUARE11 | SQUARE12 | SQUARE21 | SQUARE22);
		pieceLibrary[i++] = (ushort) (SQUARE12 | SQUARE13 | SQUARE22 | SQUARE23);
		pieceLibrary[i++] = (ushort) (SQUARE13 | SQUARE14 | SQUARE23 | SQUARE24);

		//for a simpler game set only the first 27 pieces. 

		//L & J shapes
		pieceLibrary[i++] = (ushort) (SQUARE11 | SQUARE12 | SQUARE13 | SQUARE23);
		pieceLibrary[i++] = (ushort) (SQUARE12 | SQUARE13 | SQUARE14 | SQUARE24);
		pieceLibrary[i++] = (ushort) (SQUARE11 | SQUARE12 | SQUARE13 | SQUARE21);
		pieceLibrary[i++] = (ushort) (SQUARE12 | SQUARE13 | SQUARE14 | SQUARE22);

		pieceLibrary[i++] = (ushort) (SQUARE21 | SQUARE22 | SQUARE23 | SQUARE33);
		pieceLibrary[i++] = (ushort) (SQUARE22 | SQUARE23 | SQUARE24 | SQUARE34);
		pieceLibrary[i++] = (ushort) (SQUARE21 | SQUARE22 | SQUARE23 | SQUARE31);
		pieceLibrary[i++] = (ushort) (SQUARE22 | SQUARE23 | SQUARE24 | SQUARE32);

		pieceLibrary[i++] = (ushort) (SQUARE21 | SQUARE22 | SQUARE23 | SQUARE13);
		pieceLibrary[i++] = (ushort) (SQUARE22 | SQUARE23 | SQUARE24 | SQUARE14);
		pieceLibrary[i++] = (ushort) (SQUARE21 | SQUARE22 | SQUARE23 | SQUARE11);
		pieceLibrary[i++] = (ushort) (SQUARE22 | SQUARE23 | SQUARE24 | SQUARE12);

		//Z & S shapes
		pieceLibrary[i++] = (ushort) (SQUARE11 | SQUARE12 | SQUARE22 | SQUARE23);
		pieceLibrary[i++] = (ushort) (SQUARE12 | SQUARE13 | SQUARE23 | SQUARE24);
		pieceLibrary[i++] = (ushort) (SQUARE21 | SQUARE22 | SQUARE32 | SQUARE33);
		pieceLibrary[i++] = (ushort) (SQUARE22 | SQUARE23 | SQUARE33 | SQUARE34);

		pieceLibrary[i++] = (ushort) (SQUARE12 | SQUARE13 | SQUARE21 | SQUARE22);
		pieceLibrary[i++] = (ushort) (SQUARE13 | SQUARE14 | SQUARE22 | SQUARE23);
		pieceLibrary[i++] = (ushort) (SQUARE22 | SQUARE23 | SQUARE31 | SQUARE32);
		pieceLibrary[i++] = (ushort) (SQUARE23 | SQUARE24 | SQUARE32 | SQUARE33);

		//T shapes
		pieceLibrary[i++] = (ushort) (SQUARE11 | SQUARE12 | SQUARE13 | SQUARE22);
		pieceLibrary[i++] = (ushort) (SQUARE12 | SQUARE13 | SQUARE14 | SQUARE23);
		pieceLibrary[i++] = (ushort) (SQUARE21 | SQUARE22 | SQUARE23 | SQUARE32);
		pieceLibrary[i++] = (ushort) (SQUARE22 | SQUARE23 | SQUARE24 | SQUARE33);
		pieceLibrary[i++] = (ushort) (SQUARE21 | SQUARE22 | SQUARE23 | SQUARE12);
		pieceLibrary[i++] = (ushort) (SQUARE22 | SQUARE23 | SQUARE24 | SQUARE13);


	}


	/**
	 * Creates a random initial Tile.  It will be one of the primary colors.   
	 **/
	public VirtualTile() {
		ushort filledGrid = (ushort) pieceLibrary[Random.Range(0, pieceLibrary.Length)];
		switch (Random.Range (0, 3)) 
		{
			case 0:
				redComponent = filledGrid;
				break;
			case 1:
				yellowComponent = filledGrid;
				break;
			case 2: 
				blueComponent = filledGrid;
				break;
		}
	}
		
	public VirtualTile(ushort red, ushort yellow, ushort blue)  {
		this.redComponent = red;
		this.yellowComponent = yellow;
		this.blueComponent = blue;
	}

	static bool isOn (ushort input, ushort mask)
	{
		return (input & mask) == mask;
	}

	/**
	 * 11 12 13 14       41 31 21 11
     * 21 22 23 24       42 32 22 12
     * 31 32 33 34 +---> 43 33 23 13
     * 41 42 43 44       44 34 24 14
	 */
	private ushort rotate90DegreesClockwise(ushort input) {
		ushort results = 0;
		results |= isOn (input, SQUARE41) ? SQUARE11 : ZERO;
		results |= (input & SQUARE31) == SQUARE31 ? SQUARE12 : ZERO;
		results |= (input & SQUARE21) == SQUARE21 ? SQUARE13 : ZERO;
		results |= (input & SQUARE11) == SQUARE11 ? SQUARE14 : ZERO;

		results |= (input & SQUARE42) == SQUARE42 ? SQUARE21 : ZERO;
		results |= (input & SQUARE32) == SQUARE32 ? SQUARE22 : ZERO;
		results |= (input & SQUARE22) == SQUARE22 ? SQUARE23 : ZERO;
		results |= (input & SQUARE12) == SQUARE12 ? SQUARE24 : ZERO;

		results |= (input & SQUARE43) == SQUARE43 ? SQUARE31 : ZERO;
		results |= (input & SQUARE33) == SQUARE33 ? SQUARE32 : ZERO;
		results |= (input & SQUARE23) == SQUARE23 ? SQUARE33 : ZERO;
		results |= (input & SQUARE13) == SQUARE13 ? SQUARE34 : ZERO;

		results |= (input & SQUARE44) == SQUARE44 ? SQUARE41 : ZERO;
		results |= (input & SQUARE34) == SQUARE34 ? SQUARE42 : ZERO;
		results |= (input & SQUARE24) == SQUARE24 ? SQUARE43 : ZERO;
		results |= (input & SQUARE14) == SQUARE14 ? SQUARE44 : ZERO;

		return results;
	}

	private ushort rotate180DegreesClockwise(ushort input) {
		return rotate90DegreesClockwise (rotate90DegreesClockwise (input));
	}

	private ushort rotate270DegreesClockwise(ushort input) {
		return rotate90DegreesClockwise (rotate180DegreesClockwise (input));
	}

	public VirtualTile reoriented(VirtualTile.Orientation orientation) {
		switch (orientation) {
			case Orientation.Up:
				return new VirtualTile(redComponent, yellowComponent, blueComponent);
			case Orientation.Clockwise90:
				return new VirtualTile (rotate90DegreesClockwise (redComponent), 
					rotate90DegreesClockwise (yellowComponent),
					rotate90DegreesClockwise (blueComponent));
			case Orientation.UpsideDown:
				return new VirtualTile (rotate180DegreesClockwise (redComponent), 
					rotate180DegreesClockwise (yellowComponent),
					rotate180DegreesClockwise (blueComponent));
			case Orientation.CounterClockwise90:
				return new VirtualTile (rotate270DegreesClockwise (redComponent), 
					rotate270DegreesClockwise (yellowComponent),
					rotate270DegreesClockwise (blueComponent));
		}
		return this;
	}

	public bool canMerge (VirtualTile cardPlayed, VirtualTile.Orientation orientation) {
		VirtualTile c = cardPlayed.reoriented (orientation);
		bool results = true;

		if ((redComponent & c.redComponent) != ZERO) {
			results = false;
		}
		if ((yellowComponent & c.yellowComponent) != ZERO) {
			results = false;
		}
		if ((blueComponent & c.blueComponent) != ZERO) {
			results = false;
		}
	
		return results;
	}

	public void merge(VirtualTile cardPlayed, VirtualTile.Orientation orientation) {
		//todo what about illegal moves? 
		//should we not merge?  throw exception
		VirtualTile c = cardPlayed.reoriented (orientation);
		this.redComponent |= c.redComponent;
		this.yellowComponent |= c.yellowComponent;
		this.blueComponent |= c.blueComponent;

		//unset anywhere a primary was merged with a previous secondary value.
		ushort allThreeComponents = (ushort) (redComponent & yellowComponent & blueComponent);
		redComponent &= (ushort) (~allThreeComponents);
		yellowComponent &= (ushort) (~allThreeComponents);
		blueComponent &= (ushort)  (~allThreeComponents);
	}

	public Color colorAt(ushort location) {
		if (isOn (redComponent, location) && isOn (blueComponent, location)) {
			return violet;
		}
		if (isOn (redComponent, location) && isOn (yellowComponent, location)) {
			return orange;
		}
		if (isOn (yellowComponent, location) && isOn (blueComponent, location)) {
			return green;
		} 
		if (isOn (redComponent, location)) {
			return red;
		}
		if (isOn (blueComponent, location)) {
			return blue;
		}
		if (isOn (yellowComponent, location)) {
			return yellow;
		}
		return colorless;
	}

	public int Score() {
		return HammingWeight (redComponent) + HammingWeight (blueComponent) + HammingWeight (yellowComponent);
	}

	public int HammingWeight(int value)
	{
		value = value - ((value >> 1) & 0x55555555);
		value = (value & 0x33333333) + ((value >> 2) & 0x33333333);
		return (((value + (value >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
	}
}
