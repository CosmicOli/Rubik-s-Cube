using System;
using System.Collections.Generic;
using System.Text;

namespace Rubix_Cube
{
    public class Cube
    {
        // A jagged array would do the same thing as this struct, however I felt it was clearer to illustrate the net as its own thing.
        struct Net
        {
            public Net(Cube cube)
            {
                string[,] top, bottom, left, right, back, front;

                top = new string[cube.cubeX, cube.cubeY];
                bottom = new string[cube.cubeX, cube.cubeY];

                left = new string[cube.cubeZ, cube.cubeY];
                right = new string[cube.cubeZ, cube.cubeY];

                back = new string[cube.cubeX, cube.cubeZ];
                front = new string[cube.cubeX, cube.cubeZ];

                Array[] faces = {top, bottom, left, right, back, front};

                for (int x = 0; x < cube.cubeX; x++)
                {
                    for (int y = 0; y < cube.cubeX; y++)
                    {
                        for (int z = 0; z < cube.cubeX; z++)
                        {
                            /* if x = 0, left 
                             * if x = cubeX - 1, right 
                             * if y = 0, back 
                             * if y = cubeY - 1, front 
                             * if z = 0, top
                             * if z = cubeZ - 1, bottom
                             */
                        }
                    }
                }
            }
        }

        int cubeX, cubeY, cubeZ;
        Piece[,,] cubePieces;

        public Cube(int cubeX, int cubeY, int cubeZ)
        {
            this.cubeX = cubeX;
            this.cubeY = cubeY;
            this.cubeZ = cubeZ;
            cubePieces = new Piece[cubeX, cubeY, cubeZ];
        }

        public void AssembleCube()
        {
            for (int x = 0; x < cubeX; x++)
            {
                for (int y = 0; y < cubeY; y++)
                {
                    for (int z = 0; z < cubeZ; z++)
                    {
                        // If all coordinates are extremes, it must be a corner.
                        if ((x == 0 || x == cubeX - 1) && (y == 0 || y == cubeY - 1) && (z == 0 || z == cubeZ - 1))
                        {
                            // It's rotation is relative to its correct rotation. Each additional 1 to the rotation means another twist of the corner piece.
                            cubePieces[x, y, z] = new Corner(x, y, z, 0);
                        }
                        // Checks if the piece is on the outside of the cube. If not, return the piece as null.
                        else if (x != 0 && x != cubeX - 1 && y != 0 && y != cubeY - 1 && z != 0 && z != cubeZ - 1)
                        {
                            cubePieces[x, y, z] = null;
                        }
                        // Not the nicest code I've written. It asks whether 2 of the 3 coordinates lay not an edge, and if so it is a center.
                        else if ((x != 0 && x != cubeX - 1 && y != 0 && y != cubeY - 1) || (x != 0 && x != cubeX - 1 && z != 0 && z != cubeZ - 1) || (y != 0 && y != cubeY - 1 && z != 0 && z != cubeZ - 1))
                        {
                            cubePieces[x, y, z] = new Center(x, y, z, 0);
                        }
                        else
                        {
                            cubePieces[x, y, z] = new Edge(x, y, z, 0);
                        }
                    }
                }
            }
        }

        public void ColourCube()
        {
            for (int x = 0; x < cubeX; x++)
            {
                for (int y = 0; y < cubeY; y++)
                {
                    for (int z = 0; z < cubeZ; z++)
                    {
                        if (cubePieces[x, y, z] != null)
                        {
                            cubePieces[x, y, z].AutoAssignColours(cubeX, cubeY, cubeZ);
                        }
                    }
                }
            }
        }

        private void LayerTurn(int layerDirection, int layerNumber, int rotationModifier)
        {
            Piece[,] subArray = null;

            /* For layerDirection:
             * 0 = x
             * 1 = y
             * 2 = z
             */

            switch (layerDirection)
            {
                case 0:
                    subArray = new Piece[cubeY, cubeZ];
                    for (int i = 0; i < cubeY; i++)
                    {
                        for (int j = 0; j < cubeZ; j++)
                        {
                            subArray[i,j] = cubePieces[layerNumber, i, j];
                        }
                    }
                    break;

                case 1:
                    subArray = new Piece[cubeX, cubeZ];
                    for (int i = 0; i < cubeX; i++)
                    {
                        for (int j = 0; j < cubeZ; j++)
                        {
                            subArray[i, j] = cubePieces[i, layerNumber, j];
                        }
                    }
                    break;

                case 2:
                    subArray = new Piece[cubeX, cubeY];
                    for (int i = 0; i < cubeX; i++)
                    {
                        for (int j = 0; j < cubeY; j++)
                        {
                            subArray[i, j] = cubePieces[i, j, layerNumber];
                        }
                    }
                    break;
            }

            /* Each piece should have a distance from center (x + y away). 
             * When rotated, each piece maintains that distance from the center.
             * A function then rotates the pieces relative coordinates.
             * (y, -x)
             * This rotates the pieces anticlockwise.
             */

            int centerX;
            int centerY;
            int modifierX = 0;
            int modifierY = 0;
            // An even length sided cube just functions like the odd sized cube of the size up, but it requires changing each relative coordinate to match that higher cube.

            if (subArray.GetLength(0) % 2 == 1)
            {
                centerX = (subArray.GetLength(0) - 1) / 2;
            }
            else
            {
                centerX = subArray.GetLength(0) / 2;
                modifierX = 1;
            }

            if (subArray.GetLength(1) % 2 == 1)
            {
                centerY = (subArray.GetLength(1) - 1) / 2;
            }
            else
            {
                centerY = subArray.GetLength(1) / 2;
                modifierY = 1;
            }

            Piece[,] rotatedSubArray = new Piece[subArray.GetLength(0), subArray.GetLength(1)];
            for (int x = 0; x < subArray.GetLength(0); x++)
            {
                for (int y = 0; y < subArray.GetLength(1); y++)
                {
                    int relativeX;
                    if (x < centerX)
                    {
                        relativeX = x - centerX;
                    }
                    else
                    {
                        relativeX = x + modifierX - centerX;
                    }

                    int relativeY;
                    if (y < centerY)
                    {
                        relativeY = y - centerY;
                    }
                    else
                    {
                        relativeY = y + modifierY - centerY;
                    }

                    int newRelativeX = relativeY;
                    int newRelativeY = -relativeX;

                    int newX;
                    if (newRelativeX > centerX)
                    {
                        newX = newRelativeX - modifierX + centerX;
                    }
                    else
                    {
                        newX = newRelativeX + centerX;
                    }

                    int newY;
                    if (newRelativeY > centerY)
                    {
                        newY = newRelativeY - modifierY + centerY;
                    }
                    else
                    {
                        newY = newRelativeY + centerY;
                    }

                    rotatedSubArray[newX, newY] = subArray[x, y];
                }
            }

            switch (layerDirection)
            {
                case 0:
                    for (int i = 0; i < cubeY; i++)
                    {
                        for (int j = 0; j < cubeZ; j++)
                        {
                            cubePieces[layerNumber, i, j] = rotatedSubArray[i, j];
                            if (cubePieces[layerNumber, i, j].GetTypeAsString() == "Corner" || cubePieces[i, layerNumber, j].GetTypeAsString() == "Center")
                            {
                                cubePieces[layerNumber, i, j].ChangeRotation(rotationModifier);
                            }
                            else if (cubePieces[layerNumber, i, j].GetTypeAsString() == "Edge")
                            {
                                cubePieces[layerNumber, i, j].ChangeRotation(rotationModifier / 2);
                            }
                        }
                    }
                    break;

                case 1:
                    for (int i = 0; i < cubeX; i++)
                    {
                        for (int j = 0; j < cubeZ; j++)
                        {
                            cubePieces[i, layerNumber, j] = rotatedSubArray[i, j];
                            if (cubePieces[i, layerNumber, j].GetTypeAsString() == "Corner" || cubePieces[i, layerNumber, j].GetTypeAsString() == "Center")
                            {
                                cubePieces[i, layerNumber, j].ChangeRotation(rotationModifier);
                            }
                            else if (cubePieces[i, layerNumber, j].GetTypeAsString() == "Edge")
                            {
                                cubePieces[i, layerNumber, j].ChangeRotation(rotationModifier / 2);
                            }
                        }
                    }
                    break;

                case 2:
                    for (int i = 0; i < cubeX; i++)
                    {
                        for (int j = 0; j < cubeY; j++)
                        {
                            cubePieces[i, j, layerNumber] = rotatedSubArray[i, j];
                        }
                    }
                    break;
            }
        }

        // The turn type has 3 characters. Character one is the type of rotation, character two is the layer number (if applicable), and character three is whether it is "prime" (').
        public void Turn(string turnType, int numberOfRotations)
        {
            while (turnType.Length < 3)
            {
                turnType += ' ';
            }

            char[] referenceArray = { 'l', 'm', 'r', 'b', 's', 'f', 'u', 'e', 'd' };

            char layerLetter = turnType[0];
            int xyz = Array.IndexOf(referenceArray, layerLetter) / 3;

            /* For xyz:
             * 0 = x
             * 1 = y
             * 2 = z
             */

            // If not x, corners are rotated by one if not prime, and -1 if prime.
            // If not x, edges are rotated by half if not prime, and -0.5 if prime. -1 is equivalent to 1, but are distinctly opposites. -1.5 however, is equivalent to 0.5.

            int layerNumber = 0;
            int[] sideLengths = { cubeX, cubeY, cubeZ };
            if (turnType[1] != ' ')
            {
                layerNumber = Convert.ToInt32(turnType[1]) - 48;
            }
            else if (Array.IndexOf(referenceArray, layerLetter) % 3 == 2)
            {
                layerNumber = sideLengths[xyz] - 1;
            }

            int rotationModifier = 0;
            if (xyz != 2)
            {
                rotationModifier = 1;
            }

            for (int repetition = 0; repetition < numberOfRotations; repetition++)
            {
                if (numberOfRotations == 2)
                {
                    LayerTurn(xyz, layerNumber, rotationModifier);
                    LayerTurn(xyz, layerNumber, rotationModifier);
                    break;
                }
                else if (layerNumber == sideLengths[xyz] - 1)
                {
                    if (turnType[2] == '\'')
                    {
                        LayerTurn(xyz, layerNumber, rotationModifier);
                        LayerTurn(xyz, layerNumber, rotationModifier);
                        LayerTurn(xyz, layerNumber, rotationModifier);
                    }
                    else
                    {
                        LayerTurn(xyz, layerNumber, rotationModifier);
                    }
                }
                else
                {
                    if (turnType[2] == '\'')
                    {
                        LayerTurn(xyz, layerNumber, -rotationModifier);
                    }
                    else
                    {
                        LayerTurn(xyz, layerNumber, -rotationModifier);
                        LayerTurn(xyz, layerNumber, -rotationModifier);
                        LayerTurn(xyz, layerNumber, -rotationModifier);
                    }
                }
            }
        }
    }
}
