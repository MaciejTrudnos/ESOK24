import json
from flask import Flask, request
from transformers import pipeline
from waitress import serve

model_path = "radlab/polish-qa-v2"

question_answerer = pipeline("question-answering", model=model_path)

context = """Skrót ESOK24 oznacza Elektroniczny System Obsługi Klienta.
Możesz monitorować czas wypożyczenia oraz podsumować koszt jego trwania.
Aplikacja pozwala na przeglądanie zakończonych wypożyczeń.
Możesz wypożyczyć kilka produktów w ramach jednego wypożyczenia.
Aplikacja posiada moduł raportów.
Raporty zawierają: ilość wypożyczeń, stan magazynowy, przychód roczny, podsumowaniem bieżącego miesiąca.
Jeżeli posiadasz pracowników, możesz im dodać osobne konta. 
Pracownik może jedynie wypożyczać produkty, natomiast nie ma on dostępu do magazynu i raportów.
Aplikacja dostosowana jest do wszystkich rodzajów wypożyczalni.
Określasz produkt i koszt jego wypożyczenia.
Aplikacja jest całkowicie darmowa.
Aplikacja pozwala na monitorowania stanu magazynowego i śledzenia wypożyczeń.
Niestety aplikacja nie oferuje funkcji powiadomień dla klientów.
Aby uzyskac pomoc napisz na biuro@esok24.pl
Adres email do nas to biuro@esok24.pl
Jeszcze nie umożliwiamy integracji z innymi narzędziami lub platformami.
Tak, aplikacja oferuje funkcje raportowania.
Nie posiadamy systemu ocen i opinii klientów.
Nie musisz podpisywać żadnej umowy.
Rerejstracja konta jest darmowa.
Możesz logować się na swoje konto z różnych miejsc i urządzeń.
Możesz dodawać i usuwać pracowników w dowolnym momencie.
Pracownicy nie mogą modyfikować produktów.
Aplikacja wykorzystuje chmurę obliczeniową Oracle Cloud Infrastructure.
W aplikacji wykorzystujemy mechanizmy uwierzytelniania i autoryzacji.
Aplikacja działą również na smartfonach.
Tak, możemy pomóc we wdrożeniu.
Z aplikacji korzysta kazdy kto potrzebuje ułatiwć sobie proces wypożyczania sprzętu.
Masz dostęp do szczegółowych raportów i analiz, aby dynamicznie reagować na zmieniający się rynek.
Aplikacja jest tylko w języku Polskim.
Nie musisz podawać danych osobowych klienta.
Aplikacja umożliwia zarządzanie zadaniami i harmonogramami.
Aplikacja obsługuje ceny netto i brutto w złotówkach.
Wybierz produkt i kliknij wypożycz.
Aby się zalogować wejdź przejdź adres: https://app.esok24.pl/login .
Aby się zarejestrować przejdź pod adres: https://app.esok24.pl/register .
Aby się założyć konto przejdź pod adres: https://app.esok24.pl/register .
Aby przywrócić hasło przejdź pod adres: https://app.esok24.pl/forgotpassword .
Nie trzeba podawać numeru telefonu podczas zakładania konta.
"""

app = Flask(__name__)

@app.route("/", methods = ['POST'])
def answer():    
    request_data = request.json
    result = question_answerer(question=request_data["Question"], context=context.replace("\n", " "))
    data_result = {
        "score": result["score"],
        "start": result["start"],
        "end": result["end"],
        "answer": result["answer"].strip()
    }
    json_result = json.dumps(data_result, ensure_ascii=False)
    return json_result
    
if __name__ == "__main__":
    serve(app, listen='*:5000')