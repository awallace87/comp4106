import unittest
import neuron as neur

class TestNeuron(unittest.TestCase):

    def test_neuron_init(self):
        num_inputs = 10
        test_neuron = neur.Neuron(num_inputs)
        self.assertEqual(len(test_neuron.input_weights), num_inputs)
        self.assertEqual(test_neuron.neuron_type, neur.NeuronType.Hidden, msg = "NeuronType is Wrong")
        

if __name__ == '__main__':
    unittest.main()
