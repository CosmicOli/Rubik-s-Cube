using System;
using System.Collections.Generic;
using System.Text;

namespace Rubix_Cube
{
    public abstract class Piece
    {
        protected int numberOfFaces, rotation;
        protected string[] faceColours;
        protected int startingX, startingY, startingZ;

        public Piece(int numberOfFaces)
        {
            this.numberOfFaces = numberOfFaces;
            faceColours = new string[numberOfFaces];
        }

        public abstract void AutoAssignColours(int cubeX, int cubeY, int cubeZ);

        public void ManualAssignColours(string[] passedInColours)
        {
            for (int i = 0; i < numberOfFaces; i++)
            {
                faceColours[i] = passedInColours[i];
            }
        }

        public abstract void ChangeRotation(int rotationAmount);

        public abstract string GetTypeAsString();
    }

    // Each piece is created in a default orientation, then put into its correct position.
    // Standard cube orientation has white top and blue front.
    // A corner is created like the top-back-left piece, assigning the colours in the order.
    // An edge piece is created like the top-back-middle piece, assigning the colours top-back.
    // A centre piece is only a single colour, and hence does not have a colour assigning order.

    public class Corner : Piece
    {
        public Corner(int startingX, int startingY, int startingZ, int rotation) : base(3)
        {
            this.startingX = startingX;
            this.startingY = startingY;
            this.startingZ = startingZ;
            this.rotation = rotation;
        }

        public override void AutoAssignColours(int cubeX, int cubeY, int cubeZ)
        {
            string top;
            string back;
            string left;

            // I don't particularly like coding every case but I can only see it feasable that way.
            
            if (startingZ == 0)
            {
                top = "white";
                if (startingX == startingY)
                {
                    if (startingX == 0)
                    {
                        back = "green";
                        left = "red";
                    }
                    else
                    {
                        back = "blue";
                        left = "orange";
                    }
                }
                else
                {
                    if (startingX == 0)
                    {
                        back = "red";
                        left = "blue";
                    }
                    else
                    {
                        back = "orange";
                        left = "green";
                    }
                }
            }
            else
            {
                top = "yellow";
                if (startingX == startingY)
                {
                    if (startingX == 0)
                    {
                        back = "red";
                        left = "green";
                    }
                    else
                    {
                        back = "orange";
                        left = "blue";
                    }
                }
                else
                {
                    if (startingX == 0)
                    {
                        back = "blue";
                        left = "red";
                    }
                    else
                    {
                        back = "green";
                        left = "orange";
                    }
                }
            }
            faceColours[0] = top;
            faceColours[1] = back;
            faceColours[2] = left;
        }

        public override void ChangeRotation(int rotationAmount)
        {
            rotation += rotationAmount;

            if (rotation < 0)
            {
                rotation += 3;
            }
            else if (rotation > 3)
            {
                rotation -= 3;
            }
        }

        public override string GetTypeAsString()
        {
            return "Corner";
        }
    }

    public class Edge : Piece
    {
        public Edge(int startingX, int startingY, int startingZ, int rotation) : base(2)
        {
            this.startingX = startingX;
            this.startingY = startingY;
            this.startingZ = startingZ;
            this.rotation = rotation;
        }

        public override void AutoAssignColours(int cubeX, int cubeY, int cubeZ)
        {
            string top;
            string back;

            if (startingY == 0)
            {
                back = "green";
                top = FrontAndBack();
            }
            else if (startingY != cubeY - 1)
            {
                if (startingZ == 0)
                {
                    top = "white";
                }
                else
                {
                    top = "yellow";
                }

                if (startingX == 0)
                {
                    back = "red";
                }
                else
                {
                    back = "orange";
                }
            }
            else
            {
                back = "blue";
                FrontAndBack();
            }

            faceColours[0] = top;
            faceColours[1] = back;

            string FrontAndBack()
            {
                if (startingZ == 0)
                {
                    top = "white";
                }
                else if (startingZ != cubeZ - 1)
                {
                    if (startingX == 0)
                    {
                        top = "red";
                    }
                    else
                    {
                        top = "orange";
                    }
                }
                else
                {
                    top = "yellow";
                }
                return top;
            }
        }

        public override void ChangeRotation(int rotationAmount)
        {
            rotation += rotationAmount;

            if (rotation < -1)
            {
                rotation += 2; 
            }
            else if (rotation > 1)
            {
                rotation -= 2;
            }
        }

        public override string GetTypeAsString()
        {
            return "Edge";
        }
    }

    public class Center : Piece
    {
        public Center(int startingX, int startingY, int startingZ, int rotation) : base(1)
        {
            this.startingX = startingX;
            this.startingY = startingY;
            this.startingZ = startingZ;
            this.rotation = rotation;
        }

        public override void AutoAssignColours(int cubeX, int cubeY, int cubeZ)
        {
            string faceColour;
            if (startingX == 0)
            {
                faceColour = "red";
            }
            else if (startingX == cubeX - 1)
            {
                faceColour = "orange";
            }    
            else if (startingY == 0)
            {
                faceColour = "green";
            }
            else if (startingY == cubeY - 1)
            {
                faceColour = "blue";
            }
            else if (startingZ == 0)
            {
                faceColour = "white";
            }
            else
            {
                faceColour = "yellow";
            }
            faceColours[0] = faceColour;
        }

        public override void ChangeRotation(int rotationAmount)
        {
            rotation += rotationAmount;

            if (rotation < 0)
            {
                rotation += 4;
            }
            else if (rotation > 4)
            {
                rotation -= 4;
            }
        }

        public override string GetTypeAsString()
        {
            return "Center";
        }
    }
}
