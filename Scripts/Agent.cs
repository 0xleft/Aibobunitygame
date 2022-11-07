using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    private NeuralNetwork neuralNetwork;
    public int numberOfSensors = 20;
    private int numberOfOutputs = 5; //NOTE // 1. jump 2. turn left 3. turn right 4. go forward 5. go backward 
    // Start is called before the first frame update
    [Range (0, 1)]
    public float jumpThreshold = 0.7f;
    [Range (0, 1)]
    public float turnThreshold = 0.7f;
    [Range (0, 1)]
    public float moveThreshold = 0.7f;
    private Rigidbody rb;
    public float jumpHeight = 10;
    public float moveSpeed = 10;
    private bool grounded = true;
    void Start(){
        //get rigidbody of the agent
        rb = GetComponent<Rigidbody>();

        //make agents sensors
        makeAgentSensors();

        //create brains for agent
        List<int> brainStructure = new List<int>();
        brainStructure.Add(numberOfSensors);
        brainStructure.Add(30);
        brainStructure.Add(20);
        brainStructure.Add(numberOfOutputs);
        neuralNetwork = new NeuralNetwork(brainStructure);
    }

    // Update is called once per frame
    void Update(){

        grounded = Physics.Raycast(transform.position, Vector3.down, .5f);
        //
        //if (predicting) {
            List<float> neuralPrediction = NeuralNetwork.feedForward(getSensorData(), this.neuralNetwork);

            //retreive all the values from our neural network
            float jumpp = neuralPrediction[0];
            float left = neuralPrediction[1];
            float right = neuralPrediction[2];
            float forward = neuralPrediction[3];
            float back = neuralPrediction[4];

            //jump
            if (jumpp > jumpThreshold){
                if (grounded){ // make this different line so we can give rewards acordingly
                    jump();
                }
                Debug.Log("JUMP; GROUNDED: "+grounded);
            }

            //turn left or right
            if (left > turnThreshold || right > turnThreshold){
                rotate(left, right);
                Debug.Log("TURN");
            }
            
            //go back and forward
            if (forward > moveThreshold || back > moveThreshold){
                rb.AddForce(new Vector3(), ForceMode.Acceleration);
                Debug.Log("MOVE");
            }
        //}
    }

    private void makeAgentSensors(){
    }

    private List<float> getSensorData(){
        return getRandomSensorData();
    }

    private List<float> getRandomSensorData(){
        List<float> sensorData = new List<float>();
        for (int i = 0; i < numberOfSensors; i++){
            sensorData.Add(Random.Range(0, 1));
        }
        return sensorData;
    }

    private void jump(){
        rb.AddForce(new Vector3(0, 1*jumpHeight, 0), ForceMode.Force);
    }

    private void rotate(float left, float right){
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, (right-left), 0) * Time.deltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
    
    private void move(float forward, float backward){
        //move code
    }
}


class NeuralNetwork {
    private List<Layer> layers;
    public NeuralNetwork(List<int> neuronCounts) {
        for (int i = 0; i < neuronCounts.Count-1; i++){
            Debug.Log("NEURONS IN SELECTED LAYER: "+neuronCounts[i]+" next layer: "+neuronCounts[i+1]);
            Layer newLayer = new Layer(neuronCounts[i], neuronCounts[i+1]);
            layers.Add(newLayer);
        }
        Debug.Log("BRAIN ON!! LAYER COUNT: "+layers.Count);
    }

    public static List<float> feedForward(List<float> givenInputs, NeuralNetwork neuralNetwork){
        List<float> outputs = Layer.feedForward(givenInputs, neuralNetwork.layers[0]);
        for (int i = 0; i < neuralNetwork.layers.Count; i++){
            outputs = Layer.feedForward(outputs, ((Layer)neuralNetwork.layers[i]));
        }
        return outputs;
    }

}


class Layer {
    //make a layer in the neural network
    private List<float> inputs;
    private List<float> outputs;
    private List<float> biases;
    private List<List<float>> weights;
    public Layer(int InputsCount, int OutputsCount){
        inputs = new List<float>(InputsCount);
        outputs = new List<float>(OutputsCount);
        biases = new List<float>(OutputsCount);

        weights = new List<List<float>>();
        for (int i = 0; i < InputsCount; i++)
        {
            weights.Add(new List<float>(OutputsCount));
        }

        randomize(this);
    }

    //randomize all the weights and biases
    private static void randomize(Layer layer){
        for (int i = 0; i < layer.inputs.Count; i++){
            for (int j = 0; j < layer.outputs.Count; j++){   
                layer.weights[i].Add(Random.Range(-1,1));
            }
        }
        for (int i = 0; i < layer.biases.Count; i++){
            layer.biases.Add(Random.Range(-1, 1));
        }
    }
    
    //feed forward on the neural network
    public static List<float> feedForward(List<float> givenInputs, Layer layer){
        for (int i = 0; i < layer.inputs.Count; i++){
            layer.inputs[i] = givenInputs[i];
        }

        for (int i = 0; i < layer.outputs.Count; i++){
            float sum = 0;
            for (int j = 0; j < layer.inputs.Count; j++){
                sum+=layer.inputs[j] * layer.weights[j][i];
            }
            if(sum > layer.biases[i]){
                layer.outputs[i] = 1;
            } else {
                layer.outputs[i] = 0;
            }
        }
        
        return layer.outputs;
    }
}