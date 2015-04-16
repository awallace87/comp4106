import random
import numpy as np
from enum import enum

class ActivationFunction(Enum):
    soft_max = 1
    hyper_tan = 2
    soft_max_naive = 3

def hypertan(x):
    if x < -20.0:
        return -1.0
    elif x > 20.0:
        return 1.0
    else:
        return math.tanh(x)

def softmaxnaive(o_sums):
    div = 0
    for i in range(len(o_sums)):
        div = div + math.exp(o_sums[i])
    result = [0 for i in range(len(o_sums))]
    for i in range(len(o_sums)):
        result[i] = math.exp(o_sums[i]) / div
    return result

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
    elif activation_func == ActivationFunction.soft_max_naive:
        return softmaxnaive(active_val)
    else:
        return hypertan(active_val)


class Neuron(object):
    def __init__(self, n_inputs):
        self.n_inputs = n_inputs
        self.init_weights()
        self.bias = random.uniform(0.0, 1.0)
        self.activation = ActivationFunction.hyper_tan

    def init_weights(self):
        self.weights = [random.uniform(0.0, 1.0) for i in range(self.n_inputs)]

    def forward(self, inputs):
        body_sum = 0
        for elem in [ a*b for a,b in zip(inputs,self.weights)]:
            body_sum += elem
        #self.output =
        return body_sum

class NeuronLayer(object):
    def __init__(self, n_inputs, n_nodes):
        self.n_inputs = n_inputs
        self.n_nodes = n_nodes
        self.init_nodes()

    def init_nodes(self):
        self.nodes = []
        for i in range(self.n_nodes):
            self.nodes.append(Neuron(self.n_inputs))

    def forward(self, inputs):
        outputs = []
        for node in self.nodes:
            outputs.append(node.forward(inputs))
        return outputs

class NeuralNetwork(object):
    def __init__(self, n_inputs, n_hidden_layers, n_outputs):
        self.n_inputs = n_inputs
        self.n_hidden_layers = n_hidden_layers
        self.n_outputs = n_outputs
        self.init_network()

    def init_network(self):
        self.layers = []
        input_layer = NeuronLayer(1, self.n_inputs)
        self.layers.append(input_layer)
        for i in range(self.n_hidden_layers):
            hidden_layer = NeuronLayer(self.n_inputs, self.n_inputs - i)
            self.layers.append(hidden_layer)
        output_layer = NeuronLayer(self.n_inputs, self.n_outputs)
        self.layers.append(output_layer)

    def forward(self, inputs):
        input_layer = self.layers[0]
        outputs = []
        for i in range(len(inputs)):
            original_input = [inputs[i]]
            outputs.append(input_layer.nodes[i].forward(original_input))
        for i in range(self.n_hidden_layers):
            hidden_layer = self.layers[i+1]
            outputs = hidden_layer.forward(outputs)
        output_layer = self.layers[self.n_hidden_layers + 1]
        final_output = output_layer.forward(outputs)
        return final_output

    def train(self, train_data, max_epochs, learn_rate, momentum):
        epoch = 0
        while epoch < max_epochs:

    def input(self, inputs):


num_inputs = 10
num_nodes = 4
testNeuron = Neuron(num_inputs)

print testNeuron.weights

#Test Forward
sample_inputs = np.array(random.sample(range(20), num_inputs))

print testNeuron.forward(sample_inputs)

#Test Layer
test_layer = NeuronLayer(num_inputs, num_nodes)
print test_layer.forward(sample_inputs)

#Test NeuralNetwork
neural_net = NeuralNetwork(10, 3, 1)
print(neural_net.forward(sample_inputs))
