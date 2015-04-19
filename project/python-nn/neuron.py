import random
import numpy as np
import math
from enum import Enum

MOMENTUM_CONSTANT = 0.1
#using a larger learning rate to compensate for the small training set
LEARNING_RATE = 0.7

def derivative_of(activation_func, input_val):
    if activation_func == ActivationFunction.soft_max:
        return( (1 - input_val) * input_val )
    else: #Default is HyperTan
        return( (1 - input_val) * (1 + input_val) )

def hypertan(x):
    if x < -20.0:
        return -1.0
    elif x > 20.0:
        return 1.0
    else:
        return math.tanh(x)

def softmax(o_sums):
    m = max(o_sums)
    scale = 0
    for i in range(len(o_sums)):
        scale = scale + (math.exp(o_sums[i] - m))
    result = [0 for i in range(len(o_sums))]
    for i in range(len(o_sums)):
        result[i] = math.exp(o_sums[i] - m) / scale
    return result

def GetActivationValue(activation_func, active_val):
    if activation_func == ActivationFunction.hyper_tan:
        return hypertan(active_val)
    elif activation_func == ActivationFunction.soft_max:
        return softmax(active_val)
    else:
        return hypertan(active_val)

class NeuronType(Enum):
    Hidden = 1
    Output = 2

class Neuron(object):
    def __init__(self, n_inputs, neuron_type = NeuronType.Hidden):
        self.n_inputs = n_inputs
        self.neuron_type = neuron_type
        self.init_weights()

        self.inputs = [0.0 for i in range(self.n_inputs)]
        self.output = 0
        self.gradient = 0
        self.bias = 0
        self.deltas = [0.0 for i in range(self.n_inputs)]
        self.bias_delta = 0

    def init_weights(self):
        self.weights = [random.uniform(0.0, 1.0) for i in range(self.n_inputs)]

    def forward(self, inputs):
        self.previous_inputs = inputs
        inputs_with_weights = [ a*b for a,b in zip(inputs,self.weights)]
        output_val = 0
        output_sum = 0
        for elem in inputs_with_weights:
            output_sum += elem
        if(self.neuron_type == NeuronType.Hidden):
            output_val = hypertan(output_sum + self.bias)
        else:
            output_val = output_sum + self.bias
        self.output = output_val
        return self.output

    def update_weights(self):
        for i in range(len(self.inputs)):
            delta = LEARNING_RATE * self.inputs[i] * self.gradient
            momentum = self.deltas[i] * MOMENTUM_CONSTANT
            self.weights[i] += (delta + momentum)
            self.deltas[i] = delta

    def update_bias(self):
        delta = LEARNING_RATE * self.gradient
        self.bias += delta + (self.bias_delta * MOMENTUM_CONSTANT)
        self.bias_delta = delta

    def print_neuron(self):
        print("Neuron Info: Gradient: ", self.gradient, "\nWeights - ")
        for weight in self.weights:
            print("|", weight)



class NeuronLayer(object):
    def __init__(self, n_inputs, n_nodes, layer_type = NeuronType.Hidden):
        self.n_inputs = n_inputs
        self.n_nodes = n_nodes
        self.layer_type = layer_type
        self.init_nodes()

    def init_nodes(self):
        self.nodes = []
        for i in range(self.n_nodes):
            self.nodes.append(Neuron(self.n_inputs, self.layer_type))

    def forward(self, inputs):
        outputs = []
        for node in self.nodes:
            outputs.append(node.forward(inputs))
        return outputs

    def update_weights(self):
        for node in self.nodes:
            node.update_weights()

    def update_biases(self):
        for node in self.nodes:
            node.update_bias()

    def print_layer(self):
        print("Layer - ", self.layer_type)
        for node in self.nodes:
            node.print_neuron()

class NeuralNetwork(object):
    def __init__(self, n_inputs, n_hidden_layers, n_outputs):
        self.n_inputs = n_inputs
        self.n_hidden_layers = n_hidden_layers
        self.n_outputs = n_outputs
        self.init_network()

    def init_network(self):
        self.hidden_layers = []
        for i in range(self.n_hidden_layers):
            hidden_layer = NeuronLayer(self.n_inputs, self.n_inputs)
            self.hidden_layers.append(hidden_layer)
        self.output_layer = NeuronLayer(self.n_inputs, self.n_outputs, NeuronType.Output)

    def forward(self, inputs):
        outputs = []
        for i in range(self.n_hidden_layers):
            hidden_layer = self.hidden_layers[i]
            outputs = hidden_layer.forward(inputs)
        final_output = softmax(self.output_layer.forward(outputs))
        for i in range(len(self.output_layer.nodes)):
            self.output_layer.nodes[i].output = final_output[i]
        return final_output

    def update_output_gradients(self, target_values):
        for i in range(self.n_outputs):
            output_node = self.output_layer.nodes[i]
            derivative = (1 - output_node.output) * output_node.output
            output_node.gradient = derivative * (target_values[i] - output_node.output)

    def update_hidden_gradients(self):
        previous_layer = self.output_layer
        for i in reversed(range(len(self.hidden_layers))):
            for j in range(len(self.hidden_layers[i].nodes)):
                derivative = (1 - self.hidden_layers[i].nodes[j].output) * (1 + self.hidden_layers[i].nodes[j].output)
                grad_sum = 0
                for k in range(len(previous_layer.nodes)):
                    grad_weight = previous_layer.nodes[k].gradient * previous_layer.nodes[k].weights[j]
                    grad_sum += grad_weight
                self.hidden_layers[i].nodes[j].gradient = derivative * grad_sum
            previous_layer = self.hidden_layers[i]

    def update_weights(self):
        for layer in self.hidden_layers:
            layer.update_weights()
        self.output_layer.update_weights()

    def update_biases(self):
        for layer in self.hidden_layers:
            layer.update_biases()
        self.output_layer.update_biases()

    def backward(self, target):
        self.update_output_gradients(target)
        self.update_hidden_gradients()
        self.update_weights()
        self.update_biases()

    def train(self, inputs, target):
        predicted_val = self.forward(inputs)
        #for i in range(len(predicted_val)):
        #    print("Class ", i)
        #    print("Predict - ", predicted_val[i])
        #    print("Actual Value - ", target[i])
        self.backward(target)

    def test(self, input):
        return(self.forward(input))

    def print_network(self):
        for layer in self.hidden_layers:
            layer.print_layer()
        self.output_layer.print_layer()
