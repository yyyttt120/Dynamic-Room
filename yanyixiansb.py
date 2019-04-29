import sys
sys.path.append(r'C:\Python27')
sys.path.append(r'C:\Python27\Lib')
sys.path.append(r'C:\Python27\Lib\site-packages')
sys.path.append(r'C:\Python27\DLLs')
import numpy as np
import pandas as pd

df = pd.read_csv('dataset/2.txt',delimiter=' ',header = None)
target = pd.read_csv('dataset/3.txt',header = None)

df = df.iloc[:, :-2]
df.head(20)
all_data = []

def get_text():
    return "text from hello.py"
def add(arg1, arg2):
    return arg1 + arg2
blocks = []
for index, row in df.iterrows():
    if row.isnull().all():
        all_data += [np.array([blocks])]
        blocks = []
        continue
    else:
        blocks += [np.array(row)]
        #print(row[0],index)

all_data = np.array(all_data)
all_data = all_data.reshape(all_data.shape[0],all_data.shape[2],all_data.shape[3])

target = np.array(target)

import time

#from pyqcore.client import SimpleQCoreClient
#from pyqcore.examples.jpvow import load_jpvow
from sklearn import model_selection
from sklearn.metrics import accuracy_score, f1_score
from sklearn.neural_network import MLPClassifier
from sklearn.linear_model import LogisticRegression
from sklearn import svm


    # Train: 80% / Test: 20%
X_train, X_test, y_train, y_test = model_selection.train_test_split(
        all_data, target, test_size=0.15, random_state=1
    )
X_train = X_train.reshape(len(X_train), -1).astype(np.float64)
X_test = X_test.reshape(len(X_test), -1).astype(np.float64)
y_train = np.ravel(y_train)
y_test = np.ravel(y_test)

#print("===LogisticRegression(Using Sklearn)===")
start = time.time()
lr_cls = LogisticRegression(C=0.5)
lr_cls.fit(X_train, y_train)
elapsed_time = time.time() - start
#print("elapsed_time:{0}".format(elapsed_time) + "[sec]")
res = lr_cls.predict(X=X_test)
#print("acc=", accuracy_score(y_test.tolist(), res))
#print("f1=", f1_score(y_test.tolist(), res, average="weighted"))


from joblib import dump, load
dump(lr_cls, "yanyixiansb.joblib")


