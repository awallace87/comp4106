import random
import numpy as np

class Neuron(object):
    def __init__(self, n_inputs):
        self.n_inputs = n_inputs
        self.init_weights()

    def init_weights(self):
        self.weights = [random.uniform(0.0, 1.0) for i in range(self.n_inputs)]

    def forward(self, inputs):
        body_sum = 0
        for elem in [ a*b for a,b in zip(inputs,self.weights)]:
            body_sum += elem
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
